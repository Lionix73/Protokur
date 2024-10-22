using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    private Animator anim;
    private bool fadeComplete;

    void Start()
    {
        anim = GetComponent<Animator>();
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
