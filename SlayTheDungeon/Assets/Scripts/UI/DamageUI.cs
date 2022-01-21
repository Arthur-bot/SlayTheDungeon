using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageUI : MonoBehaviour
{
    #region Fields

    [SerializeField] private TextMeshProUGUI DamageTextPrefab;

    private Canvas canvas;
    private Queue<TextMeshProUGUI> textPool = new Queue<TextMeshProUGUI>();
    private List<ActiveText> activeTexts = new List<ActiveText>();
    private Camera mainCamera;

    #endregion

    #region Protected Methods

    protected void Awake()
    {
        canvas = GetComponent<Canvas>();
        mainCamera = Camera.main;
    }

    protected void Start()
    {
        const int POOL_SIZE = 16;
        for (int i = 0; i < POOL_SIZE; ++i)
        {
            var t = Instantiate(DamageTextPrefab, canvas.transform);
            t.gameObject.SetActive(false);
            textPool.Enqueue(t);
        }
    }

    protected void Update()
    {
        for (int i = 0; i < activeTexts.Count; ++i)
        {
            var at = activeTexts[i];
            at.Timer -= Time.deltaTime;

            if (at.Timer <= 0.0f)
            {
                at.DamageText.gameObject.SetActive(false);
                textPool.Enqueue(at.DamageText);
                activeTexts.RemoveAt(i);
                i--;
            }
            else
            {
                var color = at.DamageText.color;
                color.a = at.Timer / at.MaxTime;
                at.DamageText.color = color;
                at.PlaceText(mainCamera, canvas);
            }
        }
    }

    #endregion

    #region Public Methods

    public void NewDamage(int amount, Vector3 worldPos)
    {
        var t = textPool.Dequeue();

        t.text = amount > 0? amount.ToString() : "Dodged";
        t.gameObject.SetActive(true);

        ActiveText at = new ActiveText();
        at.MaxTime = 1.0f;
        at.Timer = at.MaxTime;
        at.DamageText = t;
        at.WorldPositionStart = worldPos + Vector3.up;
        at.PlaceText(mainCamera, canvas);

        activeTexts.Add(at);
    }

    #endregion

    #region Class

    public class ActiveText
    {
        public TextMeshProUGUI DamageText { get; set; }
        public float MaxTime { get; set; }
        public float Timer { get; set; }
        public Vector3 WorldPositionStart { get; set; }

        public void PlaceText(Camera cam, Canvas canvas)
        {
            float ratio = (Timer / MaxTime);
            Vector3 pos = WorldPositionStart + new Vector3(ratio, Mathf.Sin(ratio * Mathf.PI), 0);
            pos = cam.WorldToScreenPoint(pos);
            pos *= canvas.scaleFactor;
            pos.z = 0.0f;

            DamageText.transform.position = pos;
        }
    }

    #endregion
}

