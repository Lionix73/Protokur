using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private GameObject intactObject;
    [SerializeField] private GameObject brokenObject;
    [SerializeField] private float breakThreshold;
    private Rigidbody rb;
    private AudioSource audioSource;

    void Start()
    {
        brokenObject.SetActive(false);
        rb = GameObject.Find("Player").GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (rb.velocity.magnitude > breakThreshold)
            {
                GetComponent<Collider>().enabled = false;

                brokenObject.SetActive(true);
                intactObject.SetActive(false);

                if (audioSource != null)
                {
                    audioSource.Play();
                }
            }
        }
    }
}