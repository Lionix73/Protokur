using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private Animator anim;
    private bool fadeComplete;
    private Image panelImage;

    void Start()
    {
        anim = GetComponent<Animator>();
        panelImage = GetComponent<Image>();
    }

    public void FadeOut()
    {
        anim.Play("FadeOut");
    }

    private void OnFadeComplete(){
        fadeComplete = true;
    }

    public void ResetFadeComplete(){
        fadeComplete = false;
    }

    public bool GetFadeComplete(){
        return fadeComplete;
    }
}
