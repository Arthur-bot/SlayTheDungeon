using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public abstract class BaseElementalEffect : IEquatable<BaseElementalEffect>
{
    public bool Done => timer <= 0;

    public int Timer => timer;

    public int Duration => duration;

    public Sprite EffectSprite => effectSprite;

    #region Fields

    protected int duration;
    protected int timer;
    protected CharacterData target;
    protected Sprite effectSprite;

    #endregion

    protected BaseElementalEffect(int duration)
    {
        this.duration = duration;
    }

    public virtual void Applied(CharacterData target)
    {
        timer = duration;
        this.target = target;
    }

    public virtual void Removed() { }
    public virtual void OnEqual(BaseElementalEffect other) { }

    public virtual void Update(StatSystem statSystem)
    {
        timer--;
    }

    public virtual void AddTick(int numberOfTick)
    {
        timer += numberOfTick;
    }

    public virtual void RemoveTick(int numberOfTick)
    {
        timer -= numberOfTick;
    }

    public abstract bool Equals(BaseElementalEffect other);
}

public class ElementalEffect : BaseElementalEffect
{
    private int damage;
    private StatSystem.EffectType effectType;

    public ElementalEffect(int duration, StatSystem.EffectType effectType, int damage, Sprite effectSprite) :
        base(duration)
    {
        this.damage = damage;
        this.effectType = effectType;
        this.effectSprite = effectSprite;
    }

    public override void Update(StatSystem statSystem)
    {
        base.Update(statSystem);

        statSystem.Damage(damage);
    }

    public override bool Equals(BaseElementalEffect other)
    {
        ElementalEffect eff = other as ElementalEffect;

        if (other == null)
            return false;

        return eff.effectType == effectType;
    }
}

public class PoisonEffect : BaseElementalEffect
{

    public PoisonEffect(int stack, Sprite EffectSprite) :
        base(stack)
    {
        this.effectSprite = EffectSprite;
    }

    public override void Update(StatSystem statSystem)
    {
        statSystem.Damage(timer);
        base.Update(statSystem);
    }

    public override void OnEqual(BaseElementalEffect other)
    {
        timer += other.Duration;
    }

    public override bool Equals(BaseElementalEffect other)
    {
        PoisonEffect eff = other as PoisonEffect;

        return eff != null;
    }
}

public class DodgeEffect : BaseElementalEffect
{
    public DodgeEffect(int stack, Sprite effectSprite) :
        base(stack)
    {
        this.effectSprite = effectSprite;
    }

    public override void Update(StatSystem statSystem)
    {
        
    }

    public override void OnEqual(BaseElementalEffect other)
    {
        timer += other.Duration;
    }

    public override bool Equals(BaseElementalEffect other)
    {
        DodgeEffect eff = other as DodgeEffect;

        return eff != null;
    }
}