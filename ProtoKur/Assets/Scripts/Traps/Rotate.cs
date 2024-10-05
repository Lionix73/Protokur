using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public enum RotationPattern { XAxis, YAxis, ZAxis, Custom }
    public RotationPattern rotationPattern = RotationPattern.YAxis;
    public float rotationSpeed = 10f;
    public Vector3 customAxis = Vector3.up;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotationAxis = Vector3.zero;

        switch (rotationPattern)
        {
            case RotationPattern.XAxis:
                rotationAxis = Vector3.right;
                break;
            case RotationPattern.YAxis:
                rotationAxis = Vector3.up;
                break;
            case RotationPattern.ZAxis:
                rotationAxis = Vector3.forward;
                break;
            case RotationPattern.Custom:
                rotationAxis = customAxis;
                break;
        }

        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}