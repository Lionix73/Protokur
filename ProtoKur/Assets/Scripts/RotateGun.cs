using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    private GrapplingGun grappling;
    private Quaternion desiredRotation;
    private float rotationSpeed = 5f;

    void Awake(){
        grappling = FindObjectOfType<GrapplingGun>();
    }

    void Update(){
        if(!grappling.IsGrappling()){
        desiredRotation = transform.parent.rotation;
        }
        else{
            desiredRotation = Quaternion.LookRotation(grappling.GetGrapplingPoint() - transform.position);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }
}
