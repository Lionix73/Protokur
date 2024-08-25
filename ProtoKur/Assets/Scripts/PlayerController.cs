using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Basic Movement Variables")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCoolDown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float groundDrag;
    [SerializeField] private Transform orientation;

    private bool readyToJump = true;

    [Header("Advanced Movement Variables")]
    private bool onWallR;
    private bool onWallL;


    [Header("Ground")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    private bool grounded;

    [Header("Keybinds")]
    private KeyCode jumpKey = KeyCode.Space;


    //Movement variables
    private float horizontalinput;
    private float verticalinput;

    private Vector3 moveDirection;
    private Rigidbody rb;

    private Color wallR = Color.red;
    private Color wallL = Color.red;

    //Start of the Script
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate(){
        MovePlayer();
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        WallRunCheck();

        if(grounded){
            rb.drag = groundDrag;
        }
        else{
            rb.drag = 0;
        }

        Inputs();
        SpeedControl();
        WallRun();
    }

    private void Inputs(){
        horizontalinput = Input.GetAxisRaw("Horizontal");
        verticalinput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(jumpKey) && readyToJump && grounded){
            
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCoolDown);
        }
    }

    private void MovePlayer(){
        moveDirection = orientation.forward * verticalinput + orientation.right * horizontalinput;

        if(grounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        else if(!grounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed){
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump(){
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump(){
        readyToJump = true;
    }

    private void WallRunCheck(){
        onWallR = Physics.Raycast(orientation.transform.position, orientation.right, 1f, whatIsWall);
        Debug.DrawRay(orientation.transform.position, orientation.right, wallR);
        if(onWallR){
            wallR = Color.green;
        }
        else{
            wallR = Color.red;
        }

        onWallL = Physics.Raycast(orientation.transform.position, -orientation.right, 1f, whatIsWall);
        Debug.DrawRay(orientation.transform.position, -orientation.right, wallL);
        if(onWallL){
            wallL = Color.green;
        }
        else{
            wallL = Color.red;
        }
    }

    private void WallRun(){
        if(onWallL || onWallR){
            Debug.Log("Chocando tilin");
        }
    }
}
