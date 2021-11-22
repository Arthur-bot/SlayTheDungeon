using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform toFollow;
    private Vector3 toFollowStartPos;
    private Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
        toFollowStartPos = toFollow.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPos + toFollow.position - toFollowStartPos;
    }
}
