using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logofloating : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float floatAmplitude = 30.0f;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newHeight = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newHeight, startPosition.z);
    }
}
