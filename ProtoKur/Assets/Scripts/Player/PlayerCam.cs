using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("Mouse Settings")]

    [Tooltip("Mouse X axis sens")]
    [SerializeField] private float sensX;

    [Tooltip("Mouse Y axis sens")]
    [SerializeField] private float sensY;

    [SerializeField] private Transform orientation;

    [Header("Z Rotation Wallrun")]
    [Tooltip("For The Camera Transition Bewteen 0 and the value for inclination")]
    [SerializeField] private float rotationSmoothness = 0.1f;

    private float xRotation;
    private float yRotation;

    //Just for WallRun or Effects
    private float zRotation;
    private float targetZRotation;

    void Start()
    {
        zRotation = 0f;
        targetZRotation = 0f;

        //Cursor status
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Mouse Input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 80f);

        //For Transitioning the Z Axis
        zRotation = Mathf.Lerp(zRotation, targetZRotation, rotationSmoothness);

        //Rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public float ZRotation{
        get { return targetZRotation; }
        set { targetZRotation = value; }
    }
}
