using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    #region Fields

    [SerializeField] protected string description;
    [SerializeField] protected Target targetType;
    [SerializeField] protected int value;

    #endregion


    #region Properties

    public string Description => description;

    public Target TargetType => targetType;

    #endregion

    #region Public Methods

    public virtual void ApplyEffect(CharacterData caster, List<Enemy> targets) { }

    public virtual void ApplyEffect(CharacterData caster, CharacterData target) { }

    public virtual Sprite GetIcon() { return null; }

    public virtual int GetEffectValue() { return 0; }

    public void EffectFromJson(EffectStructure effectStructure)
    {
        targetType = (Target)Enum.Parse(typeof(Target), effectStructure.targetType);
        value = effectStructure.value;
    }

    #endregion
}

public enum Target
{
    Self,
    SingleTarget,
    Aoe
}
