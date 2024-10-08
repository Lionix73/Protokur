
using UnityEngine;
using UnityEngine.UI;

public class GrapplingGun : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] private GameObject player;

    [Header("Gun Raycast Variables")]
    [SerializeField] private LayerMask whatIsGrappleable;
    [SerializeField] private Transform gunCannon;
    [SerializeField] private Transform camera;
    [SerializeField] private float maxDistance;

    [Header("Feedback Variables")]
    [SerializeField] private Image crosshairUI;
    [SerializeField] private Sprite normalCrosshair;
    [SerializeField] private Sprite lockedCrosshair;
    
    private LineRenderer lr;
    private Vector3 grapplePoint;
    private SpringJoint joint;

    void Awake(){
        lr = GetComponent<LineRenderer>();

        player = GameObject.Find("Player");
    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            StartGrapple();
        }
        else if(Input.GetMouseButtonUp(0)){
            StopGrapple();
        }

        //Debug.DrawLine(camera.position, camera.position + camera.transform.forward * maxDistance, Color.red);

        UpdateCrosshair();
    }

    void LateUpdate(){
        DrawRope();
    }

    private void UpdateCrosshair(){
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.transform.forward, out hit, maxDistance, whatIsGrappleable))
        {
            crosshairUI.transform.localScale = new Vector3(4f, 4f, 4f);
            crosshairUI.color = Color.red;
            crosshairUI.sprite = lockedCrosshair;
            crosshairUI.transform.position = Camera.main.WorldToScreenPoint(hit.collider.bounds.center); // Move crosshair to the center of the object
        }
        else
        {
            crosshairUI.transform.localScale = new Vector3(1f, 1f, 1f);
            crosshairUI.color = Color.white;
            crosshairUI.sprite = normalCrosshair;
            crosshairUI.transform.localPosition = Vector3.zero;
        }
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
