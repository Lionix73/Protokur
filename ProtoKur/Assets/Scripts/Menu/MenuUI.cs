using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject mainMenuPanel;

    void Start(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        creditsPanel.SetActive(false);
    }

    public void Play(){
        SceneManager.LoadScene("Tutorial");
    }

    public void Credits(){
        creditsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void GoBackCredits(){
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void Exit(){
        Application.Quit();
    }
}
