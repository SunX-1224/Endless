using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{

    [SerializeField] PlayerController player;
    
    float initialFOV = 69f;
    float zoomFOV = 85f;
    Vector3 offset;

    Camera cam;
    bool fppMode;

    void Start(){
        fppMode= PlayerPrefs.GetInt("fpp", 0)>0;
        if(fppMode) offset = new Vector3(0f, 0.152f, 0.397f);
        else offset = new Vector3(0f, 0.6f, -1.4f);

        cam = GetComponent<Camera>();
        cam.fieldOfView = initialFOV;
    }

    void Update(){

        cam.fieldOfView = Mathf.Lerp(initialFOV, zoomFOV, (player.rb.velocity.z - player.minVelocity)/(player.maxVelocity - player.minVelocity));
        TiltCamera(player.targetTilt);
        transform.position = player.transform.position + offset;
    }

    public void TiltCamera(Vector3 targetTilt){
        targetTilt *= 0.3f;
        if(!fppMode) targetTilt.x = 9.54f;
        Quaternion targetRotation = Quaternion.Euler(targetTilt);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3f * Time.deltaTime);
    }
}
