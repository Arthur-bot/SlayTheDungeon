using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

[CreateAssetMenu(fileName = "newEnnemyData", menuName = "EnnemyData")]
public class EnnemyData : ScriptableObject
{
    #region Fields
    [SerializeField] private Sprite sprite;
    [SerializeField] private List<CardEffect> attacks;
    [SerializeField] private StatSystem stats;
    [SerializeField] private List<CardData> ennemyDeck;
    [SerializeField] private bool boss;

    #endregion

    #region Properties

    public Sprite Sprite => sprite;
    public List<CardEffect> Attacks { get => attacks; set => attacks = value; }
    public StatSystem Stats { get => stats; set => stats = value; }
    public bool Boss { get => boss; set => boss = value; }
    public List<CardData> EnnemyDeck { get => ennemyDeck; set => ennemyDeck = value; }

    #endregion

    #region Public Methods

    public void Attack()
    {
        int indexAttack = Random.Range(0, attacks.Count);
        var attack = attacks[indexAttack];
        switch (attack.TargetTyPe)
        {
            case CardEffect.Target.Aoe:
                attack.ApplyEffect(GameManager.Instance.BattleGround.Enemies);
                break;
            case CardEffect.Target.Self:
                attack.ApplyEffect(TargetingSystem.Instance.getTarget());
                break;
            case CardEffect.Target.SingleTarget:
                attack.ApplyEffect(GameManager.Instance.Player);
                break;

        }
    }
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnnemyData))]
public class EnnemyEditor : Editor
{
    EnnemyData _target;

    SerializedProperty _spriteProperty;
    List<string> _availableEffectType;
    SerializedProperty _equippedEffectListProperty;
    SerializedProperty _statsProperty;
    SerializedProperty _deckProperty;

    void OnEnable()
    {
        _target = target as EnnemyData;
        _equippedEffectListProperty = serializedObject.FindProperty("attacks");
        _statsProperty = serializedObject.FindProperty("stats");
        _spriteProperty = serializedObject.FindProperty("sprite");
        _deckProperty = serializedObject.FindProperty("ennemyDeck");

        var lookup = typeof(CardEffect);
        _availableEffectType = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
            .Select(type => type.Name)
            .ToList();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_spriteProperty);
        EditorGUILayout.PropertyField(_statsProperty);
        _target.Boss = GUILayout.Toggle(_target.Boss, "Boss");
        if (!_target.Boss)
        {
            int choice = EditorGUILayout.Popup("Add new Attack", -1, _availableEffectType.ToArray());

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
            _target.Boss = false;
        }
        else
        {
            EditorGUILayout.PropertyField(_deckProperty);
            _target.Boss = true;
        }
    }
}
#endif