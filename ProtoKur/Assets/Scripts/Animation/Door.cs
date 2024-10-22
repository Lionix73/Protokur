using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Tooltip("If true, the door will open when the player is nearby and close when goes away.")]
    [SerializeField] private bool doorNearby;

    [Tooltip("If true, the door will stay open on trigger.")]
    [SerializeField] private bool stayOpen = false;

    [Tooltip("If true, the door will stay closed on trigger.")]
    [SerializeField] private bool stayClosed = false;

    private bool _doorNearby;
    private bool _stayOpen;
    private bool _stayClosed;

    private bool closed = true;

    private Animator anim;
    private AudioSource audioSource;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if(stayClosed)
        {
            anim.SetTrigger("Open");
            closed = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            if ((stayOpen || doorNearby) && closed)
            {
                anim.SetTrigger("Open");
                closed = false;

                if (audioSource != null)
                {
                    audioSource.Play();
                }
            }
            else if (stayClosed && !closed)
            {
                anim.SetTrigger("Close");
                closed = true;

                if (audioSource != null)
                {
                    audioSource.Play();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerCollider") && doorNearby && !closed)
        {
            anim.SetTrigger("Close");
            closed = true;

            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }

    public bool DoorNearby
    {
        get => _doorNearby;
        set
        {
            if (value)
            {
                _stayOpen = false;
                _stayClosed = false;
            }
            _doorNearby = value;
        }
    }

    public bool StayOpen
    {
        get => _stayOpen;
        set
        {
            if (value)
            {
                _doorNearby = false;
                _stayClosed = false;
            }
            _stayOpen = value;
        }
    }

    public bool StayClosed
    {
        get => _stayClosed;
        set
        {
            if (value)
            {
                _doorNearby = false;
                _stayOpen = false;
            }
            _stayClosed = value;
        }
    }
}
