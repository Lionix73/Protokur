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

    void Start()
    {
        pausePanel.SetActive(false);

        playerCamera = FindObjectOfType<PlayerCam>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            TogglePause();
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
            SceneManager.LoadScene(nextLevel);
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
