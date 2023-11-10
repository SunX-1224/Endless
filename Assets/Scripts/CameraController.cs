using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float initialFOV = 60f;
    [SerializeField] float zoomFOV = 75f;

    Vector3 offset = new(0f, 0.4f, -1.2f);

    private Coroutine fovCoroutine; 

    Camera cam;
    Transform playerTransform;
    PlayerController playerController;
    void Start(){
        cam = GetComponent<Camera>();
        cam.fieldOfView = initialFOV;

        GameObject _pl = GameObject.Find("Player");
        playerTransform = _pl.GetComponent<Transform>();
        playerController = _pl.GetComponent<PlayerController>();
    }

    void Update(){
        if(playerController.player.boostActive) IncreaseFOV(playerController.player.thrust+ playerController.player.boost);
        else ResetFOV(3.0f);
        TiltCamera(playerController.player.targetTilt, playerController.player.turnPower);
        transform.position = playerTransform.position + offset;
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
        Quaternion targetRotation = Quaternion.Euler(targetTilt);
        transform.localRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }
}
