using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject playPanel;


    void Start(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        creditsPanel.SetActive(false);
        playPanel.SetActive(false);

        creditsPanel.SetActive(false);
    }

    public void Play(){
        playPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void GoBackPlay(){
        playPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
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

    public void Tutorial(){
        SceneManager.LoadScene("Tutorial");
    }

    public void Gym(){
        SceneManager.LoadScene("TestingGym");
    }
}
