using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class StatSystem
{
    public enum EffectType
    {
        NORMAL,
        POISON,
        BLEEDING,
    }

    [System.Serializable]
    public class Stats
    {
        public int Health;
        public int Armor;
        public int Strength;
        public int Agility;
        public int Fury;

        //use an array indexed by the EffectType enum for easy extensibility
        public int[] elementalProtection = new int[Enum.GetValues(typeof(EffectType)).Length];
        public int[] elementalBoosts = new int[Enum.GetValues(typeof(EffectType)).Length];

        public void Copy(Stats other)
        {
            Health = other.Health;
            Armor = other.Armor;
            Strength = other.Strength;
            Agility = other.Agility;
            Fury = other.Fury;

            Array.Copy(other.elementalProtection, elementalProtection, other.elementalProtection.Length);
            Array.Copy(other.elementalBoosts, elementalBoosts, other.elementalBoosts.Length);
        }

        public void Modify(StatModifier modifier)
        {
            //bit convoluted, but allow to reuse the normal int stat system for percentage change
            if (modifier.ModifierMode == StatModifier.Mode.PERCENTAGE)
            {
                Health += Mathf.FloorToInt(Health * (modifier.Stats.Health / 100.0f));
                Armor += Mathf.FloorToInt(Armor * (modifier.Stats.Armor / 100.0f));
                Strength += Mathf.FloorToInt(Strength * (modifier.Stats.Strength / 100.0f));
                Agility += Mathf.FloorToInt(Agility * (modifier.Stats.Agility / 100.0f));
                Fury += Mathf.FloorToInt(Fury * (modifier.Stats.Fury / 100.0f));

                for (int i = 0; i < elementalProtection.Length; ++i)
                    elementalProtection[i] += Mathf.FloorToInt(elementalProtection[i] * (modifier.Stats.elementalProtection[i] / 100.0f));

                for (int i = 0; i < elementalBoosts.Length; ++i)
                    elementalBoosts[i] += Mathf.FloorToInt(elementalBoosts[i] * (modifier.Stats.elementalBoosts[i] / 100.0f));
            }
            else
            {
                Health += modifier.Stats.Health;
                Armor += modifier.Stats.Armor;
                Strength += modifier.Stats.Strength;
                Agility += modifier.Stats.Agility;
                Fury += modifier.Stats.Fury;

                for (int i = 0; i < elementalProtection.Length; ++i)
                    elementalProtection[i] += modifier.Stats.elementalProtection[i];

                for (int i = 0; i < elementalBoosts.Length; ++i)
                    elementalBoosts[i] += modifier.Stats.elementalBoosts[i];
            }
        }
    }

    [System.Serializable]
    public class StatModifier
    {
        public enum Mode
        {
            PERCENTAGE,
            ABSOLUTE
        }

        public Mode ModifierMode = Mode.ABSOLUTE;
        public Stats Stats = new Stats();
    }

    /// <summary>
    /// This is a special StatModifier, that gets added to the TimedStatModifier stack, that will be automatically
    /// removed when its timer reaches 0. Contains a StatModifier that controls the actual modification.
    /// </summary>
    [System.Serializable]
    public class TimedStatModifier
    {
        public string Id;
        public StatModifier Modifier;

        public Sprite EffectSprite;

        public int Duration;
        public int Timer;

        public void Reset()
        {
            Timer = Duration;
        }
    }

    #region Fields

    [SerializeField] private Stats baseStats;

    private CharacterData owner;

    private List<StatModifier> modifiersStack = new List<StatModifier>();
    private List<TimedStatModifier> timedModifierStack = new List<TimedStatModifier>();
    private List<BaseElementalEffect> elementalEffects = new List<BaseElementalEffect>();

    #endregion

    #region Properties

    public Stats BaseStats => baseStats;

    public Stats StatsCopy { get; set; } = new Stats();

    public int CurrentHealth { get; private set; }

    public int CurrentArmor { get; private set; }
    public int CurrentFury { get; private set; }

    public List<BaseElementalEffect> ElementalEffects => elementalEffects;

    public List<TimedStatModifier> TimedModifierStack => timedModifierStack;

    #endregion

    #region Delegates

    public delegate void OnHitEventHandler(CharacterData gameCharacter);
    public event OnHitEventHandler OnHit;

    #endregion

    #region Public Methods

    public void Init(CharacterData owner)
    {
        StatsCopy.Copy(BaseStats);
        CurrentHealth = StatsCopy.Health;
        this.owner = owner;
    }

    public void AddModifier(StatModifier modifier)
    {
        modifiersStack.Add(modifier);
        UpdateFinalStats();
    }

    public void RemoveModifier(StatModifier modifier)
    {
        modifiersStack.Remove(modifier);
        UpdateFinalStats();
    }

    public void AddTimedModifier(StatModifier modifier, int duration, string id, Sprite sprite)
    {
        bool found = false;
        int index = timedModifierStack.Count;
        for (int i = 0; i < timedModifierStack.Count; ++i)
        {
            if (timedModifierStack[i].Id == id)
            {
                found = true;
                index = i;
            }
        }

        if (!found)
        {
            timedModifierStack.Add(new TimedStatModifier() { Id = id });
        }

        timedModifierStack[index].EffectSprite = sprite;
        timedModifierStack[index].Duration = duration;
        timedModifierStack[index].Modifier = modifier;
        timedModifierStack[index].Reset();

        UpdateFinalStats();
        RaiseOnHit();
    }

    public void AddElementalEffect(BaseElementalEffect effect)
    {
        effect.Applied(owner);
        var dodged = false;

        // Check if can dodge
        if (BattleData.Instance.CurrentTarget != Target.Self)
        {
            for (var i = 0; i < elementalEffects.Count; ++i)
            {
                var _effect = elementalEffects[i];

                if (!(_effect is DodgeEffect)) continue;

                _effect.RemoveTick(1);
                dodged = true;

                if (!_effect.Done) continue;

                elementalEffects[i].Removed();
                elementalEffects.RemoveAt(i);
                i--;
            }
        }
        if (dodged) return;

        // Apply effect
        var effectExist = false;
        for (int i = 0; i < elementalEffects.Count; ++i)
        {
            if (effect.Equals(elementalEffects[i]))
            {
                effectExist = true;
                elementalEffects[i].OnEqual(effect);
            }
        }

        if (!effectExist)
            elementalEffects.Add(effect);

        RaiseOnHit();
    }

    public void OnDeath()
    {
        ClearEffect();

        UpdateFinalStats();
    }

    public void Tick()
    {
        var needUpdate = false;

        for (int i = 0; i < timedModifierStack.Count; ++i)
        {
            if (timedModifierStack[i].Timer > 0)
            {
                timedModifierStack[i].Timer--;
                if (timedModifierStack[i].Timer <= 0)
                {
                    //modifier finished, so we remove it from the stack
                    timedModifierStack.RemoveAt(i);
                    i--;
                    needUpdate = true;
                }
            }
        }

        if (needUpdate)
            UpdateFinalStats();

        for (int i = 0; i < elementalEffects.Count; ++i)
        {
            var effect = elementalEffects[i];
            effect.Update(this);

            if (effect.Done)
            {
                elementalEffects[i].Removed();
                elementalEffects.RemoveAt(i);
                i--;
            }
        }

        RaiseOnHit();
    }

    public int ChangeHealth(int amount)
    {
        int damageTaken = 0;
        if(amount < 0)
        {
            //Takes damage
            var lifeDamage = amount + CurrentArmor;
            ChangeArmor(amount);
            if (lifeDamage < 0)
            {
                damageTaken = -lifeDamage;
                CurrentHealth = Mathf.Clamp(CurrentHealth + lifeDamage, 0, StatsCopy.Health);
            }
        }
        else
        {
            //healing
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, StatsCopy.Health);
        }

        RaiseOnHit();
        return damageTaken;
    }

    public void ChangeArmor(int amount)
    {
        CurrentArmor = Mathf.Clamp(CurrentArmor + amount, 0, StatsCopy.Armor);

        RaiseOnHit();
    }

    public void ChangeFury(int amount)
    {
        CurrentFury = Mathf.Clamp(CurrentFury + amount, 0, StatsCopy.Fury);

        RaiseOnHit();
    }

    public int Damage(int totalDamage)
    {
        if (!owner.IsAlive) return 0;

        var dodged = false;

        // Check if can dodge
        if (BattleData.Instance.CurrentTarget != Target.Self && totalDamage > 0)
        {
            for (var i = 0; i < elementalEffects.Count; ++i)
            {
                var effect = elementalEffects[i];

                if (!(effect is DodgeEffect)) continue;

                effect.RemoveTick(1);
                dodged = true;

                if (!effect.Done) continue;

                elementalEffects[i].Removed();
                elementalEffects.RemoveAt(i);
                i--;
            }
        }

        RaiseOnHit();

        var damage = dodged? 0 : ChangeHealth(-totalDamage);
        GameUI.Instance.DamageUI.NewDamage(dodged ? 0 : totalDamage, owner.transform.position);
        return damage;
    }

    public void ClearEffect()
    {
        foreach (var e in ElementalEffects)
            e.Removed();

        ElementalEffects.Clear();
        TimedModifierStack.Clear();
        RaiseOnHit();
    }

    #endregion

    #region Private Methods

    private void UpdateFinalStats()
    {
        var maxHealthChange = false;
        var previousHealth = StatsCopy.Health;

        StatsCopy.Copy(BaseStats);

        foreach (var modifier in modifiersStack)
        {
            if (modifier.Stats.Health != 0)
                maxHealthChange = true;

            StatsCopy.Modify(modifier);
        }

        foreach (var timedModifier in timedModifierStack)
        {
            if (timedModifier.Modifier.Stats.Health != 0)
                maxHealthChange = true;

            StatsCopy.Modify(timedModifier.Modifier);
        }

        //if we change the max Health we update the current Health to it's new value
        if (maxHealthChange)
        {
            var percentage = CurrentHealth / (float)previousHealth;
            CurrentHealth = Mathf.RoundToInt(percentage * StatsCopy.Health);
        }
    }

    private void RaiseOnHit() => OnHit?.Invoke(owner);

    #endregion
}

