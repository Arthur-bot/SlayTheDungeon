using System;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
    where T : Singleton<T>
{
    #region Fields

    private static T _instance;

    #endregion

    #region Properties

    public static bool HasInstance => _instance != null
#if UNITY_EDITOR
        || !Application.isPlaying && FindObjectOfType<T>()
#endif
        ;

    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;

            var singletonOptionsAttributes = typeof(T).GetCustomAttributes(typeof(SingletonOptionsAttribute), true);
            var singletonOptionsAttribute = (SingletonOptionsAttribute)(singletonOptionsAttributes.Length > 0 ? singletonOptionsAttributes[0] : null);

            var name = $"Prefabs/[{(!string.IsNullOrEmpty(singletonOptionsAttribute?.Name) ? singletonOptionsAttribute.Name.ToUpper() : typeof(T).Name.ToUpper())}]";

            var asset = FindObjectOfType<T>();
            if (asset != null)
            {
                if (Application.isPlaying)
                {
                    asset.Awake();
                }
                else
                {
                    _instance = asset;
                }

                return _instance;
            }

            GameObject go;
            if (singletonOptionsAttribute != null && singletonOptionsAttribute.IsPrefab)
            {
#if UNITY_EDITOR
                asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>($"Assets/Resources/{name}.prefab");
                go = asset.gameObject;
#else
                go = (GameObject)Resources.Load(name);
                asset = go.GetComponent<T>();
#endif
                if (asset.DontInstantiate)
                {
                    asset.Awake();
                }
                else asset = Instantiate(go).GetComponent<T>();

                _instance = asset;
            }
            else
            {
                go = new GameObject($"[{name.ToUpperInvariant()}]");

                _instance = go.GetComponent<T>();

                if (_instance == null)
                    _instance = go.AddComponent<T>();
            }

            return _instance;
        }
    }

    public virtual bool UseDontDestroyOnLoad => false;

    public virtual bool DontInstantiate => false;

    protected bool IsNotTheSingletonInstance => _instance != null && _instance != this;

    #endregion

    #region Protected Methods

    protected virtual void OnAwake() { }

    #endregion

    #region Unity Event Functions

    protected void Awake()
    {
        // For [ExecuteInEditMode] objects
        if (!Application.isPlaying) return;

        if (_instance != null)
        {
            if (_instance != this)
            {
                DestroyImmediate(gameObject);
            }

            return;
        }

        _instance = (T)this;

        if (UseDontDestroyOnLoad && !DontInstantiate) DontDestroyOnLoad(gameObject);

        OnAwake();
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    #endregion
}

[AttributeUsage(AttributeTargets.Class)]
public class SingletonOptionsAttribute : Attribute
{
    public SingletonOptionsAttribute(string name, bool isPrefab = false)
    {
        Name = name;
        IsPrefab = isPrefab;
    }

    public string Name { get; }

    public bool IsPrefab { get; }
}
