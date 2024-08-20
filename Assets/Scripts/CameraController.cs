using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{

    [SerializeField] PlayerController player;
    [SerializeField] GameObject fppOverlay;
    
    [SerializeField] float initialFOV;
    [SerializeField] float zoomFOV;
    [SerializeField] Vector3 offset;

    Camera cam;
    bool fppMode;

    void Start(){
        fppMode= PlayerPrefs.GetInt("fpp", 0)>0;
        if(fppMode) {
            offset = new Vector3(0f, 0.5f, 0.0f);
            fppOverlay.SetActive(true);
        }

        cam = GetComponent<Camera>();
        cam.fieldOfView = initialFOV;
    }

    void Update(){
        cam.fieldOfView = Mathf.Lerp(initialFOV, zoomFOV, (player.rb.velocity.z - player.minVelocity)/(player.maxVelocity - player.minVelocity));
        TiltCamera(player.targetTilt);
        transform.position = player.transform.position + offset;
    }

    public void TiltCamera(Vector3 targetTilt){
        targetTilt.z *= 0.4f;
        if(!fppMode) targetTilt.x = 0f;
        targetTilt.x += 10f;
        Quaternion targetRotation = Quaternion.Euler(targetTilt);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3f * Time.deltaTime);
    }
}
