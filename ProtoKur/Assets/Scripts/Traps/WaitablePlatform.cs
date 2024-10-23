using System.Collections;
using UnityEngine;

public class WaitablePlatform : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;

    private bool isMoving = false;
    float tolerance = 0.2f;

    private AudioSource audioSource;
    private float fadeDuration = 0.5f;

    private bool playerOnPlatform = false;
    private Transform targetPoint;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        audioSource = GetComponent<AudioSource>();

        transform.position = pointA.position;
        targetPoint = pointA;

        audioSource.volume = 0f;
    }

    private void Update(){

        if(Vector3.Distance(transform.position, pointB.position) < tolerance || Vector3.Distance(transform.position, pointA.position) < tolerance){
            isMoving = false;
        }
        else{
            isMoving = true;
        }

        HandleAudio();
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

    private void HandleAudio()
    {
        if (isMoving)
        {
            StartCoroutine(FadeIn(audioSource, fadeDuration, 1f));
        }
        else if (!isMoving)
        {
            StartCoroutine(FadeOut(audioSource, fadeDuration, 1f, false));
        }
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

    private IEnumerator FadeIn(AudioSource audioSource, float fadeDuration, float targetVolume)
    {
        float startVolume = 0f;
        audioSource.volume = startVolume;
    
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0.6f, t / fadeDuration);
            yield return null;
        }
    
        audioSource.volume = targetVolume;
    }

    private IEnumerator FadeOut(AudioSource audioSource, float fadeDuration, float startVolume, bool resetVolume = true)
    {
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        if (resetVolume){
            audioSource.Stop();
            audioSource.volume = startVolume;
        }
        else{
            audioSource.volume = 0;
        }
    }
}
