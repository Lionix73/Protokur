using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Basic Movement Variables")]
    [Min(1f)] [SerializeField] private float moveSpeed;
    [Min(1f)] [SerializeField] private float maxSpeed;
    [Min(1f)] [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCoolDown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;
    [SerializeField] private float crouchDrag;
    [Range(0.1f, 0.9f)] [SerializeField] private float crouchHeight;
    [SerializeField] private Transform orientation;

    private Vector3 crouchScale;
    private Vector3 flatVel;
    private bool crouching = false;
    private bool readyToJump = true;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    
    private float JumpBufferTime = 0.1f;
    private float JumpBufferCounter;


    [Header("Wall Movement Variables")]
    [Min(1f)] [SerializeField] private float wallRunForce;
    [SerializeField] private float minVelToWallrun;
    [SerializeField] private float wallDrag;
    [Min(0.5f)][SerializeField] private float wallJumpCoolDown;

    private bool onWallR;
    private bool onWallL;
    private bool onWallFront;
    private bool activeWallRun;


    [Header("Slide Movement Variables")]
    [Min(1f)] [SerializeField] private float slideForce;
    [SerializeField] private float slideDrag;
    [SerializeField] private float minVelToSlide;
    private bool sliding;
    

    [Header("Vaulting Variables")]
    private int vaultLayer;
    private bool vaulting = false;


    [Header("Grappling Variables")]
    [SerializeField] private GameObject grapplingGun;
    [SerializeField] private bool activeGrapple;
    private GrapplingGun grappling;


    [Header("Ground Variables")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    private bool grounded;


    [Header("Keybinds")]
    private KeyCode jumpKey = KeyCode.Space;
    private KeyCode crouch = KeyCode.LeftControl;


    [Header("Player Variables")]
    //Visual and Colliding Variables
    [SerializeField] private GameObject playerCapsule;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private GameObject tempCam;

    private Camera playerCamera;
    private PlayerCam playerCamScript;


    //Movement variables
    private float horizontalinput;
    private float verticalinput;

    private Vector3 moveDirection;
    private Rigidbody rb;

    private Color wallR = Color.red;
    private Color wallL = Color.red;
    private Color wallFront = Color.red;

    //Animations And VFX Variables
    [Header("Animations and VFX")]
    [SerializeField] private ParticleSystem speedVFX;
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float fastFOV = 75f;
    [SerializeField] private float speedThreshold = 18f;


    void Awake(){
        rb = GetComponent<Rigidbody>();

        grappling = FindObjectOfType<GrapplingGun>();

        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();  

        playerCamScript = FindObjectOfType<PlayerCam>();

        vaultLayer = LayerMask.NameToLayer("vaultLayer");
        vaultLayer = ~vaultLayer;
    }


    //Start of the Script
    void Start()
    {
        rb.freezeRotation = true;

        crouchScale = new Vector3(playerCapsule.transform.localScale.x, playerCapsule.transform.localScale.y * crouchHeight, 
        playerCapsule.transform.localScale.z);
    }

    private void FixedUpdate(){
        MovePlayer();
        rb.AddForce(Vector3.down * 10f, ForceMode.Force);
    }

    void Update()
    {
        //Debug.Log("Vel: " + flatVel.magnitude);

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsGround);

        CoyoteTimeManager();
        
        WallRunCheck();

        DragVerifications();

        Inputs();

        SpeedControl();

        if(flatVel.magnitude >= minVelToWallrun && !grounded && !activeWallRun && (onWallL || onWallR)){
            WallRun();
        }

        ExitingCameraRotation();

        TempCamRotation();

        //Mostly for Me (usless for the user)
        ActiveGrappleGun();
    }

    private void CoyoteTimeManager(){
        if(grounded){
            coyoteTimeCounter = coyoteTime;
        }
        else{
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void TempCamRotation(){
        tempCam.transform.rotation = playerCamera.transform.rotation;
    }

    private void ExitingCameraRotation(){
        if(!onWallL && !onWallR)
            activeWallRun = false;

        if(vaulting){
            playerCamScript.ZRotation = 10f;
        }
        else if(!activeWallRun && !vaulting){
            playerCamScript.ZRotation = 0f;
        } 
    }

    private void DragVerifications(){
        if(grounded && !crouching){
            rb.drag = groundDrag;
            activeWallRun = false;
        }
        else if(grounded && crouching && !sliding){
            rb.drag = crouchDrag;        
        }
        else if(!grounded){
            rb.drag = airDrag;
        }
        else if(!grounded && (onWallL || onWallR)){
            rb.drag = wallDrag;
        }
        else if (!grounded && !onWallL && !onWallR){
            rb.drag = airDrag;
            activeWallRun = false;
        }
        else{
            //Let the others work
        }
    }

    private void Inputs(){
        horizontalinput = Input.GetAxisRaw("Horizontal");
        verticalinput = Input.GetAxisRaw("Vertical");

        if(JumpBufferCounter > 0f && readyToJump && coyoteTimeCounter > 0 && !sliding){
            
            readyToJump = false;

            Jump();

            JumpBufferCounter = 0f;

            Invoke(nameof(ResetJump), jumpCoolDown);
        }

        if(Input.GetKeyDown(jumpKey) && readyToJump && activeWallRun){
            
            readyToJump = false;

            WallJump();

            Invoke(nameof(ResetWallJump), wallJumpCoolDown);
        }

        if(Input.GetKeyDown(jumpKey) && readyToJump && coyoteTimeCounter > 0 && sliding){
            readyToJump = false;

            SlideJump();

            Invoke(nameof(ResetJump), jumpCoolDown);
        }

        if(Input.GetKeyDown(jumpKey)){
            //For Jump Buffering
            JumpBufferCounter = JumpBufferTime;

            //For Vaulting
            Vault();
        }
        else{
            JumpBufferCounter -= Time.deltaTime;
        }

        if(Input.GetKeyUp(jumpKey)){
            coyoteTimeCounter = 0;
        }

        if(Input.GetKeyDown(crouch) && grounded && !activeWallRun){
            Crouch();
        }

        if(Input.GetKeyDown(crouch) && !grounded && !activeWallRun){
            Crouch();
        }

        if(Input.GetKeyUp(crouch)){
            Uncrouch();
        }
    }

    private void MovePlayer(){
        moveDirection = orientation.forward * verticalinput + orientation.right * horizontalinput;

        if(grounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!grounded && !grappling.IsGrappling()){
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);
        }
        else if(!grounded && grappling.IsGrappling()){
            rb.AddForce(moveDirection.normalized * moveSpeed * 2f, ForceMode.Force);
        }
    }

    private void SpeedControl(){
        flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if(flatVel.magnitude > maxSpeed){
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        if(flatVel.magnitude > speedThreshold){
            speedVFX.Play();
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fastFOV, Time.deltaTime * 2);

        }
        else if(flatVel.magnitude < speedThreshold){
            speedVFX.Stop();
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * 2);
        }
    }

    private void Jump(){

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);  
    }

    private void SlideJump(){

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce * 1.5f, ForceMode.Impulse);

        rb.AddForce(orientation.forward * jumpForce * 5f, ForceMode.Impulse);  
    }

    private void Crouch(){        
        crouching = true;

        playerCapsule.transform.localScale = crouchScale;
        cameraPos.transform.localPosition = new Vector3 (0f, cameraPos.transform.localPosition.y * 0.5f, 0f);

        if (flatVel.magnitude >= minVelToSlide && coyoteTimeCounter > 0){
            Slide();
        }
    }

    private void Uncrouch(){
        crouching = false;
        sliding = false;

        transform.position = new Vector3(playerCapsule.transform.position.x, playerCapsule.transform.position.y + 0.05f, playerCapsule.transform.position.z);
        playerCapsule.transform.localScale = new Vector3 (playerCapsule.transform.localScale.x, crouchScale.y / crouchHeight, playerCapsule.transform.localScale.x);
        cameraPos.transform.localPosition = new Vector3 (0f, cameraPos.transform.localPosition.y / 0.5f, 0f);
    } 

    private void Slide(){
        sliding = true;
        rb.drag = slideDrag;

        rb.AddForce(orientation.forward * slideForce * 2f, ForceMode.VelocityChange);

        StartCoroutine(SlideControl());
    }

    private IEnumerator SlideControl(){
        float slideDuration = 1.5f;
        float slideTimer = 0f;

        // Desaceleración progresiva
        while (sliding && flatVel.magnitude > 0.1f) {
            slideTimer += Time.deltaTime;

            rb.drag = Mathf.Lerp(slideDrag, crouchDrag, slideTimer / slideDuration);

            // Terminar el slide después de cierto tiempo
            if (slideTimer >= slideDuration || flatVel.magnitude < 0.1f || !grounded) {
                sliding = false;
                rb.drag = crouchDrag; 
                break;
            }

            yield return null;
        }

        sliding = false;
        rb.drag = crouchDrag; 
    }

    private void WallJump(){

        if(onWallL){
            rb.AddForce(orientation.right * jumpForce * 50f, ForceMode.Force); 

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce * 1f, ForceMode.Impulse);   
        }
        if(onWallR){
            rb.AddForce(-orientation.right * jumpForce * 50f, ForceMode.Force);   

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce * 1f, ForceMode.Impulse);   
        }
    }

    private void ResetJump(){
        readyToJump = true;
    }

    private void ResetWallJump(){
        readyToJump = true;
    }

    private void WallRunCheck(){
        onWallR = Physics.Raycast(transform.position, orientation.right, 1f, whatIsWall);
        Debug.DrawRay(transform.position, orientation.right, wallR);
        if(onWallR){
            wallR = Color.green;
        }
        else{
            wallR = Color.red;
        }

        onWallL = Physics.Raycast(transform.position, -orientation.right, 1f, whatIsWall);
        Debug.DrawRay(transform.position, -orientation.right, wallL);
        if(onWallL){
            wallL = Color.green;
        }
        else{
            wallL = Color.red;
        }

        onWallFront = Physics.Raycast(transform.position, orientation.forward, 1f, whatIsWall);
        Debug.DrawRay(transform.position, orientation.forward, wallFront);
        if(onWallFront){
            wallFront = Color.green;
        }
        else{
            wallFront = Color.red;
        }
    }

    private void WallRun(){

        if(onWallL){
            playerCamScript.ZRotation = -5f;

            activeWallRun = true;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(-orientation.right * wallRunForce * 20f, ForceMode.Force);
            rb.AddForce(transform.up * wallRunForce * 20f, ForceMode.Acceleration);
            
        }
        else if(onWallR){
            playerCamScript.ZRotation = 5f;

            activeWallRun = true;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(orientation.right * wallRunForce * 20f, ForceMode.Force);
            rb.AddForce(transform.up * wallRunForce * 20f, ForceMode.Acceleration);
        }
        else if(onWallFront){

        }
    }

    private void Vault(){
        if (Physics.Raycast(cameraPos.position, orientation.transform.forward, out var firstHit, 1f, vaultLayer)){
            Debug.DrawRay(cameraPos.position, orientation.transform.forward);

            Vector3 firstHitOffset = firstHit.point + orientation.transform.forward * 0.5f + Vector3.up * 0.6f * playerHeight;
            if (Physics.Raycast(firstHitOffset, Vector3.down, out var secondHit, playerHeight)){
                vaulting = true;
                playerCamera.enabled = false;
                tempCam.SetActive(true);

                StartCoroutine(LerpVault(secondHit.point + new Vector3(0f, playerHeight / 2 + 0.1f, 0f), 0.5f));
            }
        }

    }

    IEnumerator LerpVault(Vector3 targetPosition, float duration){
        float time = 0;
        Vector3 startPosition = transform.position;

        while(time < duration){
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        playerCamera.enabled = true;
        tempCam.SetActive(false);
        transform.position = targetPosition;
        vaulting = false;
    }

    private void ActiveGrappleGun(){
        if(activeGrapple){
            grapplingGun.SetActive(true);
        }
        else{
            grapplingGun.SetActive(false);
        }
    }
}
