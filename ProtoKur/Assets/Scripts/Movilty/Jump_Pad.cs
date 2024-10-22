using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Pad : MonoBehaviour
{
    [SerializeField] private float jumpForce = 40f;

    private Rigidbody playerRb;

    void Start()
    {
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else{
            //Rigidbody rb = other.GetComponent<Rigidbody>();
            //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
