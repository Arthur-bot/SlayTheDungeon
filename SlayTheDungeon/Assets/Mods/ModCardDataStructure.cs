using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModCardDataStructure
{
    #region Fields

    [SerializeField] private Sprite sprite;
    [SerializeField] private int cost;
    [SerializeField] private string cardName;
    [SerializeField] private string description;
    [SerializeField] private AudioClip cardSoundEffect;
    [SerializeField] private List<CardEffect> cardEffects;
    [SerializeField] private bool limitedUse;
    [SerializeField] private int nbUse;

    #endregion

    #region Properties

    public Sprite Sprite => sprite;

    public int Cost => cost;

    public string CardName => cardName;

    public string Description => description;

    public List<CardEffect> CardEffects => cardEffects;

    public bool LimitedUse { get => limitedUse; set => limitedUse = value; }
    public int NbUse { get => nbUse; set => nbUse = value; }

    #endregion
}


