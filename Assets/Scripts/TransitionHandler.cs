using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionHandler : MonoBehaviour{
    
    public RawImage fadeImage;
    float fadeSpeed = -1.0f;
    bool isFading = true;

    void Start(){
        fadeImage.color = new Color(0,0,0,1f);
    }

    void Update(){
        if(isFading){
            float a = Mathf.Clamp01(fadeImage.color.a + fadeSpeed * Time.deltaTime);
            fadeImage.color = new Color(0,0,0,a);

            if(a>=1.0f) isFading = false;
        }
    }

    public void StartFadeIn(){
        isFading = true;
        fadeSpeed = 1.0f;
    }

    public bool getStatus(){
        return isFading;
    }

    public void StartFadeOut(){
        isFading = true;
        fadeSpeed = -1.0f;
    }

}

