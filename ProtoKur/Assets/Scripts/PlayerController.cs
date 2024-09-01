using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
    [Range(0.1f, 0.9f)] [SerializeField] private float crouchHeight;
    [SerializeField] private Transform orientation;

    private Vector3 crouchScale;
    private float crouchDrag;
    private Vector3 flatVel;
    private bool crouching = false;
    private bool readyToJump = true;


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
    [SerializeField] private float minVelToSlide;
    [Range(0.01f, 0.5f)][SerializeField] private float crouchDecelerationRate;
    private bool sliding;


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
    private float cameraPosHeight;


    //Movement variables
    private float horizontalinput;
    private float verticalinput;

    private Vector3 moveDirection;
    private Rigidbody rb;

    private Color wallR = Color.red;
    private Color wallL = Color.red;
    private Color wallFront = Color.red;


    //Start of the Script
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        crouchScale = new Vector3(playerCapsule.transform.localScale.x, crouchHeight, playerCapsule.transform.localScale.z);
        crouchDrag = groundDrag;
        cameraPosHeight = cameraPos.transform.localPosition.y;
    }

    private void FixedUpdate(){
        MovePlayer();
        rb.AddForce(Vector3.down * 10f, ForceMode.Force);
    }

    void Update()
    {
        if(!crouching){
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsGround);
        }
        else if(crouching){
            grounded = Physics.Raycast(crouchScale, Vector3.down, crouchHeight * 0.5f + 0.2f, whatIsGround);
        }
        
        WallRunCheck();

        DragVerifications();

        Inputs();
        SpeedControl();

        if(flatVel.magnitude >= minVelToWallrun && !grounded && !activeWallRun && (onWallL || onWallR)){
            WallRun();
        }
    }

    private void DragVerifications(){
        if(grounded && !crouching){
            rb.drag = groundDrag;
            activeWallRun = false;
        }
        else if(grounded && crouching){
            rb.drag = crouchDrag;
        }
        else if(!grounded && (onWallL || onWallR)){
            rb.drag = wallDrag;
        }
        else if (!grounded && !onWallL && !onWallR){
            rb.drag = groundDrag;
            activeWallRun = false;
        }
        else{
            rb.drag = 0f;
        }
    }

    private void Inputs(){
        horizontalinput = Input.GetAxisRaw("Horizontal");
        verticalinput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(jumpKey) && readyToJump && grounded){
            
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCoolDown);
        }

        if(Input.GetKeyDown(jumpKey) && readyToJump && activeWallRun){
            
            readyToJump = false;

            WallJump();

            Invoke(nameof(ResetWallJump), wallJumpCoolDown);
        }

        if(Input.GetKeyDown(crouch)){
            Crouch();
        }

        if (Input.GetKeyDown(crouch)){
            CrouchDeceleration();
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

        else if(!grounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);
        }
    }

    private void SpeedControl(){
        flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if(flatVel.magnitude > maxSpeed){
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump(){

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);  
    }

    private void Crouch(){
        crouching = true;

        playerCapsule.transform.localScale = crouchScale;
        cameraPos.transform.localPosition = new Vector3 (0f, crouchHeight, 0f);

        if (flatVel.magnitude >= minVelToSlide && grounded && !sliding){
            Slide();
        }
    }

    private void Uncrouch(){
        crouching = false;
        sliding = false;

        crouchDrag = groundDrag;

        playerCapsule.transform.localScale = new Vector3 (playerCapsule.transform.localScale.x, 1f, playerCapsule.transform.localScale.x);
        cameraPos.transform.localPosition = new Vector3 (0f, cameraPosHeight, 0f);
    } 

    private void Slide(){
        if (!sliding){
            rb.AddForce(orientation.forward * slideForce * 10f, ForceMode.Force);
            sliding = true;
        }
    }

    private void CrouchDeceleration(){
        StartCoroutine(nameof(CrouchDecelerationCoroutine), crouchDecelerationRate);
    }

    private IEnumerator CrouchDecelerationCoroutine(float decelerationRate){
        while (rb.drag < 16){
            crouchDrag = crouchDrag + 0.2f;
            rb.drag = crouchDrag;

            yield return new WaitForSeconds(decelerationRate);
        }
    }

    private void WallJump(){

        if(onWallL){
            rb.AddForce(orientation.right * jumpForce * 300f, ForceMode.Force); 

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce * 2f, ForceMode.Impulse);   
        }
        if(onWallR){
            rb.AddForce(-orientation.right * jumpForce * 300f, ForceMode.Force);   

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce * 2f, ForceMode.Impulse);   
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
            activeWallRun = true;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(-orientation.right * wallRunForce * 20f, ForceMode.Force);
            rb.AddForce(transform.up * wallRunForce * 60f, ForceMode.Acceleration);
            
        }
        else if(onWallR){
            activeWallRun = true;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(orientation.right * wallRunForce * 20f, ForceMode.Force);
            rb.AddForce(transform.up * wallRunForce * 60f, ForceMode.Acceleration);
        }
        else if(onWallFront){

        }
    }
}
