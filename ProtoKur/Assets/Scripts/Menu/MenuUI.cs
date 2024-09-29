using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public void Play(){
        SceneManager.LoadScene("Tutorial");
    }

    public void Credits(){
        SceneManager.LoadScene("Credits");
    }

    public void Exit(){
        Application.Quit();
    }
}
