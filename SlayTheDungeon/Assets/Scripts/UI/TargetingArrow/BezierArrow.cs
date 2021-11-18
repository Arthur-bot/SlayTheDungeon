using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierArrow : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject arrowHeadPrefab;
    [SerializeField] private GameObject arrowNodePrefab;
    [SerializeField] private int arrowNodeNbr;
    [SerializeField] private float scaleFactor = 1f;

    private RectTransform origin;
    private List<RectTransform> arrowNodes = new List<RectTransform>();
    private List<Vector2> controlPoints = new List<Vector2>();
    private List<Vector2> controlPointFactors = new List<Vector2>() { new Vector2(-0.3f, 0.8f), new Vector2(0.1f, 1.4f) };

    #endregion

    #region Protected Methods

    protected void Awake()
    {
        // Gets position of the arrow emitter
        origin = GetComponent<RectTransform>();

        // Instantiates arrow nodes and head
        for (int i = 0; i < arrowNodeNbr; i++)
        {
            arrowNodes.Add(Instantiate(arrowNodePrefab, transform).GetComponent<RectTransform>());
        }
        arrowNodes.Add(Instantiate(arrowHeadPrefab, transform).GetComponent<RectTransform>());

        // Initializes control points list
        for (int i = 0; i < 4; i++)
        {
            controlPoints.Add(Vector2.zero);
        }
    }

    protected void Update()
    {
        // P0 is at the emitter point
        controlPoints[0] = new Vector2(origin.position.x, origin.position.y);
        // P3 is at the mouse position
        controlPoints[3] = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        // P1 et P2 determined by P0 and P3
        controlPoints[1] = controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointFactors[0];
        controlPoints[2] = controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointFactors[1];

        for (int i = 0; i < arrowNodeNbr + 1; i++)
        {
            // Calculates t
            var t = Mathf.Log(1f * i / arrowNodeNbr + 1f, 2f);

            // Calculates position of each node
            arrowNodes[i].position =
                Mathf.Pow(1 - t, 3) * controlPoints[0] +
                3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1] +
                3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2] +
                Mathf.Pow(t, 3) * controlPoints[3];

            // Calculates rotation of each node
            if (i > 0)
            {
                var euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, arrowNodes[i].position - arrowNodes[i - 1].position));
                arrowNodes[i].rotation = Quaternion.Euler(euler);
            }

            // Calculates scale of each node
            var scale = scaleFactor * (1f - 0.03f * (arrowNodeNbr - i));
            arrowNodes[i].localScale = new Vector3(scale, scale, 1f);

            // Rotation of the first node
            arrowNodes[0].transform.rotation = arrowNodes[1].transform.rotation;
        }
    }

    #endregion
}

