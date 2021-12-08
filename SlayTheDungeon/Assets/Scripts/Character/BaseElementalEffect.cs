using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// The base class to derive from to write you own custom Elemental effect that can be added to a StatsSystem. There
/// is a default implementation called ElementalEffect that can be used to make Physical/Fire/Electrical/Cold damage
/// across duration.
///
/// A derived class *must* implement the Equals function so we can check if 2 effects are the same (e.g. the default
/// implementation ElementalEffect will consider 2 effect equal if they do the same DamageType).
/// </summary>
public abstract class BaseElementalEffect : IEquatable<BaseElementalEffect>
{
    public bool Done => timer <= 0.0f;
    public int CurrentTime => timer;
    public int Duration => duration;

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

/// <summary>
/// Default implementation of the BaseElementalEffect. The constructor allows the caller to specify what type of
/// damage is done, how much is done and the speed (duration) between each instance of damage (default 1 = every second).
/// </summary>
public class ElementalEffect : BaseElementalEffect
{
    private int damage;
    private StatSystem.DamageType damageType;

    public ElementalEffect(int duration, StatSystem.DamageType damageType, int damage) :
        base(duration)
    {
        this.damage = damage;
        this.damageType = damageType;
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