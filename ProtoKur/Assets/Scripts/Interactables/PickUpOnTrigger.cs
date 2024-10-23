using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpOnTrigger : MonoBehaviour
{
    [SerializeField] private Door doorToUnlock;
    private PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            // Pick up the object
            playerController.ActiveGrapple = true;

            doorToUnlock.Open();
            
            Destroy(gameObject);
        }
    }
}
