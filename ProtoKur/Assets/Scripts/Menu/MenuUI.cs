using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject playPanel;

    [Header("Fades")]
    private Fade fadeScript;

    private int sceneIndex;

    void Start(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        fadeScript = FindObjectOfType<Fade>();

        creditsPanel.SetActive(false);
        playPanel.SetActive(false);

        creditsPanel.SetActive(false);
    }

    void Update(){
        if (fadeScript.GetFadeComplete())
        {
            SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        }
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
        sceneIndex = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Tutorial.unity");

        Debug.Log(sceneIndex);

        fadeScript.FadeOut();
    }

    public void Gym(){
        sceneIndex = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/TestingGym.unity");

        Debug.Log(sceneIndex);

        fadeScript.FadeOut(); 
    }
}
