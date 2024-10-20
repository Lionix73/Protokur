using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioFeetSource;
    [SerializeField] private AudioSource audioSpeedSource;
    [SerializeField] private AudioSource audioGrapplingSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip running;
    [SerializeField] private AudioClip sliding;
    [SerializeField] private AudioClip grappling;

    [Header("Audio Cooldowns")]
    [SerializeField] private float footstepCooldown = 0.2f;
    [SerializeField] private float crouchstepCooldown = 0.3f;
    [SerializeField] private float wallRunstepCooldown = 0.15f;

    private bool isSlidingSoundPlaying = false;
    private Coroutine fadeOutCoroutine; 
    private PlayerController playerController;
    private GrapplingGun grapplingGun;
    private float lastFootstepTime;
    private bool wasFast = false;
    private bool hasGrappled = false;

    
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        grapplingGun = FindObjectOfType<GrapplingGun>();
        lastFootstepTime = -footstepCooldown;
    }
    
    void Update()
    {
        WalkingVerification(running);
        WallRunningVerification(running);
        CrouchingVerification(running);
        SlidingVerification(sliding);
        GoingFastVerification();
        GrapplingVerification();
    }
    
    private void WalkingVerification(AudioClip audio)
    {
        if (playerController.IsGrounded && playerController.IsWalking && !playerController.IsCrouching && !playerController.IsSliding)
        {
            if (Time.time >= lastFootstepTime + footstepCooldown)
            {
                audioFeetSource.pitch = Random.Range(0.8f, 1.1f);
                audioFeetSource.PlayOneShot(audio);
                lastFootstepTime = Time.time;
            }
        }
    }

    private void WallRunningVerification(AudioClip audio){
        if (playerController.IsWallRunning){
            if (Time.time >= lastFootstepTime + wallRunstepCooldown){
                audioFeetSource.pitch = Random.Range(0.8f, 1.1f);
                audioFeetSource.PlayOneShot(audio);
                lastFootstepTime = Time.time;
            }
        }
    }

    private void CrouchingVerification(AudioClip audio){
        if (playerController.IsCrouching && playerController.IsGrounded && playerController.IsWalking && !playerController.IsSliding){
            if (Time.time >= lastFootstepTime + crouchstepCooldown){
                audioFeetSource.pitch = Random.Range(0.8f, 1.1f);
                audioFeetSource.PlayOneShot(audio);
                lastFootstepTime = Time.time;
            }
        }
    }

    private void SlidingVerification(AudioClip audio)
    {
        if (playerController.IsSliding)
        {
            if (!isSlidingSoundPlaying)
            {
                audioFeetSource.volume = Random.Range(0.5f, 0.7f);
                audioFeetSource.PlayOneShot(audio);
                isSlidingSoundPlaying = true;
            }
        }
        else
        {
            if (isSlidingSoundPlaying)
            {
                if (fadeOutCoroutine != null)
                {
                    StopCoroutine(fadeOutCoroutine);
                }
                fadeOutCoroutine = StartCoroutine(FadeOut(audioFeetSource, 0.3f, 1f, true));
                isSlidingSoundPlaying = false;
            }
        }
    }

    private void GoingFastVerification()
    {
        if (playerController.IsFast && !wasFast)
        {
            wasFast = true;
            StartCoroutine(FadeIn(audioSpeedSource, 0.5f, 0.6f));
        }
        else if (!playerController.IsFast && wasFast)
        {
            wasFast = false;
            StartCoroutine(FadeOut(audioSpeedSource, 0.5f, 0.6f, false));
        }
    }

    private void GrapplingVerification()
    {
        if (grapplingGun.IsGrappling() && !hasGrappled)
        {
            hasGrappled = true;
            audioGrapplingSource.pitch = Random.Range(0.8f, 1.1f);
            audioGrapplingSource.PlayOneShot(grappling);
        }
        else if (!grapplingGun.IsGrappling() && hasGrappled)
        {
            hasGrappled = false;
        }
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
}
