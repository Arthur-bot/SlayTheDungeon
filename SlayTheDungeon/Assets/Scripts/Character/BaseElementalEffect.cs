using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BaseElementalEffect : IEquatable<BaseElementalEffect>
{
    public bool Done => timer <= 0;
    public int Timer => timer;
    public int Duration => duration;

    public Sprite EffectSprite;

    #region Fields

    protected int duration;
    protected int timer;
    protected CharacterData target;

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

    public virtual void Update(StatSystem statSystem)
    {
        timer--;
    }

    public virtual void AddDuration(int duration)
    {
        timer += duration;
    }

    public abstract bool Equals(BaseElementalEffect other);
}

public class ElementalEffect : BaseElementalEffect
{
    private int damage;
    private StatSystem.DamageType damageType;

    public ElementalEffect(int duration, StatSystem.DamageType damageType, int damage, Sprite EffectSprite) :
        base(duration)
    {
        this.damage = damage;
        this.damageType = damageType;
        this.EffectSprite = EffectSprite;
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

        return eff.damageType == damageType;
    }

    public override void Applied(CharacterData target)
    {
        base.Applied(target);

        // Update FX, UI ...
    }

    public override void Removed()
    {
        base.Removed();

        // Update FX, UI ...
    }
}