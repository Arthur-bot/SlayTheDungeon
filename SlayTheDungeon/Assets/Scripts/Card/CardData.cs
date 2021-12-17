using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

[CreateAssetMenu(fileName = "newCardData", menuName = "CardData")]
public class CardData : ScriptableObject
{
    #region Fields

    [SerializeField] private Sprite sprite;
    [SerializeField] private int cost;
    [SerializeField] private string cardName;
    [SerializeField] private string description;
    [SerializeField] private AudioClip cardSoundEffect;
    [SerializeField] private List<CardEffect> cardEffects = new List<CardEffect>();
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

    #region Public Methods

    public bool CanPlay()
    {
        // if player has enough mana/energy whatever...

        return false;
    }

    public void Use(bool isPlayer = true)
    {
        nbUse--;
        foreach (var ce in cardEffects)
        {
            switch (ce.TargetType)
            {
                case Target.Aoe:
                    ce.ApplyEffect(GameManager.Instance.BattleGround.Enemies);
                    break;
                case Target.Self:
                    ce.ApplyEffect(isPlayer? GameManager.Instance.Player : TargetingSystem.Instance.getTarget());
                    break;
                case Target.SingleTarget:
                    ce.ApplyEffect(isPlayer? TargetingSystem.Instance.getTarget() : GameManager.Instance.Player);
                    break;

            }
        }
        AudioManager.Instance.PlaySFX(cardSoundEffect);
    }

    public bool NeedTarget()
    {
        foreach (var ce in cardEffects)
        {
            if (ce.TargetType == Target.SingleTarget)
            {
                return true;
            }
        }
        return false;
    }

    public void CardFromStructure(CardStructure cardStructure)
    {
        cardName = cardStructure.cardName;
        cost = cardStructure.cost;
        description = cardStructure.description;
        limitedUse = cardStructure.limitedUse;
        nbUse = cardStructure.nbUse;

        //Effects
        foreach (var effectData in cardStructure.cardEffects)
        {
            var effect = CreateInstance(effectData.name) as CardEffect;

            if (effect == null) continue;

            effect.EffectFromJson(effectData);
            cardEffects.Add(effect);
        }

        //Sprite
        var loadedSprite = Resources.Load<Sprite>("Cards/" + cardStructure.spritePath);

        Debug.Log(cardStructure.spritePath);

        if (loadedSprite != null)
        {
            sprite = loadedSprite;
        }
    }

    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(CardData))]
public class CardEditor : Editor
{
    CardData _target;

    SerializedProperty _nameProperty;
    SerializedProperty _spriteProperty;
    SerializedProperty _descriptionProperty;
    SerializedProperty _costProperty;
    SerializedProperty _cardSoundEffectProperty;
    SerializedProperty _limitedUseProperty;
    SerializedProperty _nbUseProperty;

    List<string> _availableEffectType;
    SerializedProperty _equippedEffectListProperty;

    void OnEnable()
    {
        _target = target as CardData;
        _equippedEffectListProperty = serializedObject.FindProperty("cardEffects");
        _nameProperty = serializedObject.FindProperty("cardName");
        _spriteProperty = serializedObject.FindProperty("sprite");
        _descriptionProperty = serializedObject.FindProperty("description");
        _costProperty = serializedObject.FindProperty("cost");
        _cardSoundEffectProperty = serializedObject.FindProperty("cardSoundEffect");
        _nbUseProperty = serializedObject.FindProperty("nbUse");

        var lookup = typeof(CardEffect);
        _availableEffectType = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
            .Select(type => type.Name)
            .ToList();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_nameProperty);
        EditorGUILayout.PropertyField(_spriteProperty);
        EditorGUILayout.PropertyField(_costProperty);
        EditorGUILayout.PropertyField(_cardSoundEffectProperty);
        EditorGUILayout.PropertyField(_descriptionProperty, GUILayout.MinHeight(128));
        _target.LimitedUse = GUILayout.Toggle(_target.LimitedUse, "Limited Use");

        if (_target.LimitedUse)
        {
            _target.NbUse = EditorGUILayout.IntField("Number of uses:", _target.NbUse);
        }

        int choice = EditorGUILayout.Popup("Add new CardData Effect", -1, _availableEffectType.ToArray());

        if (choice != -1)
        {
            var newInstance = ScriptableObject.CreateInstance(_availableEffectType[choice]);

            AssetDatabase.AddObjectToAsset(newInstance, target);

            _equippedEffectListProperty.InsertArrayElementAtIndex(_equippedEffectListProperty.arraySize);
            _equippedEffectListProperty.GetArrayElementAtIndex(_equippedEffectListProperty.arraySize - 1).objectReferenceValue = newInstance;
        }


        Editor ed = null;
        int toDelete = -1;
        for (int i = 0; i < _equippedEffectListProperty.arraySize; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            var item = _equippedEffectListProperty.GetArrayElementAtIndex(i);
            SerializedObject obj = new SerializedObject(item.objectReferenceValue);

            Editor.CreateCachedEditor(item.objectReferenceValue, null, ref ed);

            ed.OnInspectorGUI();
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("-", GUILayout.Width(32)))
            {
                toDelete = i;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (toDelete != -1)
        {
            var item = _equippedEffectListProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
            DestroyImmediate(item, true);

            //need to do it twice, first time just nullify the entry, second actually remove it.
            _equippedEffectListProperty.DeleteArrayElementAtIndex(toDelete);
            _equippedEffectListProperty.DeleteArrayElementAtIndex(toDelete);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
