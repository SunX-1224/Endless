using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float initialFOV = 60f;
    [SerializeField] float zoomFOV = 75f;
    
    private Coroutine fovCoroutine; 

    Camera cam;
    void Start(){
        cam = GetComponent<Camera>();
        cam.fieldOfView = initialFOV;
    }

    public void IncreaseFOV(float speed){
        if(fovCoroutine != null){
            StopCoroutine(fovCoroutine);
        }
        fovCoroutine = StartCoroutine(ChangeFOV(zoomFOV, speed));
    }

    public void ResetFOV(float speed){
        if(fovCoroutine != null){
            StopCoroutine(fovCoroutine);
        }
        fovCoroutine = StartCoroutine(ChangeFOV(initialFOV, speed));
    }

    IEnumerator ChangeFOV(float targetFOV, float speed){
        float currentFOV = cam.fieldOfView;

        while(Mathf.Abs(currentFOV - targetFOV) > 0.01f){
            currentFOV = Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime * speed);
            cam.fieldOfView = currentFOV;
            yield return null;
        }

        cam.fieldOfView = targetFOV;
    }

}
