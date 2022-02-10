using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    string objectTag;

    float delta = 3.5f;  // Amount to move left and right from the start point
    public float speed = 2.0f;
    private Vector3 startPos;

    private void Awake()
    {
        objectTag = transform.gameObject.tag;
    }

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (objectTag == "obstacle")
        {
            Vector3 v = startPos;
            v.x -= delta * Mathf.Sin(Time.time * speed);
            transform.position = v;
        }
    }
}
