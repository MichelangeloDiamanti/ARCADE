using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAround : MonoBehaviour
{
    public Transform center;
    public Vector3 axis = Vector3.up;
    public float rotationSpeed = 80.0f;

    void Start()
    {
    }

    void Update()
    {
        transform.RotateAround(center.position, axis, rotationSpeed * Time.deltaTime);
    }
}
