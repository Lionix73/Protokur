using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioFeetSource;
    [SerializeField] private AudioSource audioSpeedSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip running;
    [SerializeField] private AudioClip sliding;

    [Header("Audio Cooldowns")]
    [SerializeField] private float footstepCooldown = 0.2f;
    [SerializeField] private float crouchstepCooldown = 0.3f;
    [SerializeField] private float wallRunstepCooldown = 0.15f;

    private bool isSlidingSoundPlaying = false;
    private Coroutine fadeOutCoroutine; 
    private PlayerController playerController;
    private float lastFootstepTime;
    
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        lastFootstepTime = -footstepCooldown; // Initialize to allow immediate first step
    }
    
    void Update()
    {
        WalkingVerification(running);
        WallRunningVerification(running);
        CrouchingVerification(running);
        SlidingVerification(sliding);
        GoingFastVerification();
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
                fadeOutCoroutine = StartCoroutine(FadeOut(audioFeetSource, 1.0f));
                isSlidingSoundPlaying = false;
            }
        }
    }

    private void GoingFastVerification(){
        if (playerController.IsFast){
            audioSpeedSource.mute = false;
        }
        else{
            audioSpeedSource.mute = true;
        }
    }

    private IEnumerator FadeOut(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
