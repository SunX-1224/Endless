using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float initialFOV = 60f;
    float zoomFOV = 75f;
    Vector3 offset = new(0f, 0.4f, -1.2f);
    
    Coroutine fovCoroutine; 
    Camera cam;
    PlayerController player = null;

    void Start(){
        cam = GetComponent<Camera>();
        cam.fieldOfView = initialFOV;
    }

    void Update(){
        if(!player) return;

        if(player.boostActive) IncreaseFOV(player.GetForce());
        else ResetFOV(3.0f);
        TiltCamera(player.targetTilt, player.GetTurnPower());
        transform.position = player.transform.position + offset;
    }

    public void SetPlayerController(PlayerController _player){
        player = _player;
    }

    void IncreaseFOV(float speed){
        if(fovCoroutine != null){
            StopCoroutine(fovCoroutine);
        }
        fovCoroutine = StartCoroutine(ChangeFOV(zoomFOV, speed));
    }

    void ResetFOV(float speed){
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

    public void TiltCamera(Vector3 targetTilt, float speed){
        targetTilt.x = transform.rotation.x;
        Quaternion targetRotation = Quaternion.Euler(targetTilt);
        transform.localRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }
}
