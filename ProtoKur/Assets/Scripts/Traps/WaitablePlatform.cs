using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitablePlatform : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;

    private bool playerOnPlatform = false;
    private Transform targetPoint;
    private GameObject player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        transform.position = pointA.position;
        targetPoint = pointA;
    }

    private void FixedUpdate()
    {
        if (playerOnPlatform)
        {
            targetPoint = pointB;
        }
        else if (transform.position == pointB.position || !playerOnPlatform)
        {
            targetPoint = pointA;
        }

        MovePlatform();
    }

    private void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            player.transform.SetParent(transform);
            playerOnPlatform = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            player.transform.SetParent(null);
            playerOnPlatform = false;
        }
    }
}
