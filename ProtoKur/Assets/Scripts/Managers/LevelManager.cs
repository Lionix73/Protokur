using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Variables")]
    [SerializeField] private string nextLevel;

    [Header("Pause Variables")]
    [SerializeField] private bool canPause = true;
    [SerializeField] private GameObject pausePanel;
    private bool isPaused = false;
    private PlayerCam playerCamera;

    [Header("FadeOut")]
    private Fade fadeScript;

    void Start()
    {
        pausePanel.SetActive(false);

        playerCamera = FindObjectOfType<PlayerCam>();

        fadeScript = FindObjectOfType<Fade>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            TogglePause();
        }

        if(fadeScript.GetFadeComplete())
        {
            SceneManager.LoadSceneAsync(nextLevel, LoadSceneMode.Single);
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        playerCamera.enabled = isPaused ? false : true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            fadeScript.FadeOut();
        }
    }

    public void GoMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume()
    {
        TogglePause();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
