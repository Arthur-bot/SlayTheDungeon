using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    #region Fields

    [SerializeField] private string description;
    [SerializeField] private Target targetType;

    #endregion


    #region Properties

    public string Description => description;

    public Target TargetTyPe => targetType;

    #endregion

    #region Public Methods

    public virtual void ApplyEffect(List<Enemy> targets) { }

    public virtual void ApplyEffect(CharacterData target) { }

    #endregion

    #region Enum

    public enum Target
    { 
        Self,
        SingleTarget,
        Aoe
    }

    #endregion
}
