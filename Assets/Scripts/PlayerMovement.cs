using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private CameraController camController;

    [SerializeField] float thrust;
    [SerializeField] float boost;
    [SerializeField] float turnPower;
    [SerializeField] float jumpPower;

    [SerializeField] Vector3 maxTilt = new Vector3(20, 0, 30);

    private Coroutine tiltCoroutine;
    private Vector3 targetTilt = new Vector3(0,0,0);

    void Start(){
        rb = GetComponent<Rigidbody>();
        camController = GetComponentInChildren<Camera>().GetComponent<CameraController>();
    }

    void FixedUpdate(){

        Vector3 force = new(0,0,0);
        force.z = thrust;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
            force.z += boost;
            camController.IncreaseFOV(boost + thrust);
        }else{
            camController.ResetFOV(3.0f);
            if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
                force.z -= boost;
                targetTilt.x = -maxTilt.x;
            }else{
                targetTilt.x = 0f;
            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            force.x -= turnPower;
            targetTilt.z = maxTilt.z;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            force.x += turnPower;
            targetTilt.z = -maxTilt.z;
        }
        else{
            targetTilt.z = 0f;
        }

        if(rb.useGravity && transform.position.y < 0f){
            rb.useGravity = false;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY;
        }

        if(Input.GetKey(KeyCode.Space)){
            if(!rb.useGravity) {
                rb.constraints = RigidbodyConstraints.FreezeRotationY;
                rb.AddForce(0,jumpPower, 0);
                rb.useGravity = true;
            }
        }

        rb.AddForce(force);
    }

    void Update(){
        TiltPlayer(targetTilt, turnPower);
    }

    void TiltPlayer(Vector3 targetTilt, float speed){
        Quaternion targetRotation = Quaternion.Euler(targetTilt);
        transform.localRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }
}

