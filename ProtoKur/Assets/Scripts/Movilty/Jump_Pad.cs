using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Pad : MonoBehaviour
{
    [SerializeField] private float jumpForce = 40f;
    private AudioSource audioSource;

    private Rigidbody playerRb;

    void Start()
    {
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            audioSource.Play();
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else{
            //Rigidbody rb = other.GetComponent<Rigidbody>();
            //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
