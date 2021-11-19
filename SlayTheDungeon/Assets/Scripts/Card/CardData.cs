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
    [SerializeField] private string cardName;
    [SerializeField] private string description;
    [SerializeField] private List<CardEffect> cardEffects;

    #endregion

    #region Properties

    public Sprite Sprite => sprite;

    public string CardName => cardName;

    public string Description => description;

    public List<CardEffect> CardEffects => cardEffects;

    #endregion

    #region Public Methods

    public bool CanPlay()
    {
        // if player has enough mana/energy whatever...

        return false;
    }

    public void Use()
    {
        foreach (var ce in cardEffects)
        {
            switch (ce.TargetTyPe)
            {
                case CardEffect.Target.Aoe:
                    ce.ApplyEffect(GameManager.Instance.Enemies);
                    break;
                case CardEffect.Target.Self:
                    ce.ApplyEffect(GameManager.Instance.Player);
                    break;
                case CardEffect.Target.SingleTarget:
                    ce.ApplyEffect(TargetingSystem.Instance.getTarget());
                    break;

            }
        }
    }

    public bool NeedTarget()
    {
        foreach (var ce in cardEffects)
        {
            if (ce.TargetTyPe == CardEffect.Target.SingleTarget)
            {
                return true;
            }
        }
        return false;
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

    List<string> _availableEffectType;
    SerializedProperty _equippedEffectListProperty;

    void OnEnable()
    {
        _target = target as CardData;
        _equippedEffectListProperty = serializedObject.FindProperty("cardEffects");

        _nameProperty = serializedObject.FindProperty("cardName");
        _spriteProperty = serializedObject.FindProperty("sprite");
        _descriptionProperty = serializedObject.FindProperty("description");

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
        EditorGUILayout.PropertyField(_descriptionProperty, GUILayout.MinHeight(128));

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
