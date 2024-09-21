using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] private GameObject player;

    [Header("Gun Raycast Variables")]
    [SerializeField] private LayerMask whatIsGrappleable;
    [SerializeField] private Transform gunCannon;
    [SerializeField] private Transform camera;
    [SerializeField] private float maxDistance;
    
    private LineRenderer lr;
    private Vector3 grapplePoint;
    private SpringJoint joint;

    void Awake(){
        lr = GetComponent<LineRenderer>();
    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            StartGrapple();
        }
        else if(Input.GetMouseButtonUp(0)){
            StopGrapple();
        }
    }

    void LateUpdate(){
        DrawRope();
    }

    private void StartGrapple(){
        RaycastHit hit;

        if(Physics.Raycast(camera.position, camera.transform.forward, out hit, maxDistance, whatIsGrappleable)){
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.transform.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
        }
    }

    private void DrawRope(){
        if (!joint) return;

        lr.SetPosition(0, gunCannon.position);
        lr.SetPosition(1, grapplePoint);
    }

    private void StopGrapple(){
        lr.positionCount = 0;
        Destroy(joint);
    }

    public bool IsGrappling(){
        return joint != null;
    }

    public Vector3 GetGrapplingPoint(){
        return grapplePoint;
    }
}
