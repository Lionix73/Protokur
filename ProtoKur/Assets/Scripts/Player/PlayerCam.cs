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

    private float xRotation;
    private float yRotation;

    void Start()
    {
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

        //Rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
