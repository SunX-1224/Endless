using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController: MonoBehaviour{

    GameManager gameManager;
    
    public bool isAlive = true;
    public int jumps;
    public int shields;

    public bool boostActive;

    public Vector3 targetTilt;
    
    Vector3 maxTilt = new Vector3(20,0, 30);
    Rigidbody rb;
    Coroutine tiltCoroutine;
    Ship ship;

    void Start(){
        gameManager = GetComponentInParent<GameManager>();
        ship = GetComponent<Ship>();
        rb = GetComponent<Rigidbody>();
    }

    void Update(){
        TiltPlayer();
    }

    void FixedUpdate(){
        HandleControls();
    }

    public float GetForce(){
        return ship.boost + ship.thrust;
    }

    public float GetTurnPower(){
        return ship.turnPower;
    }

    void HandleControls(){
        if(!isAlive) return;

        Vector3 force = new(0, 0, 0);
        force.z = ship.thrust;
        targetTilt.y = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
            force.z += ship.boost;
            boostActive = true;
        }else{
            boostActive = false;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
                force.z -= ship.boost;
                targetTilt.x = -maxTilt.x;
            }else{
                targetTilt.x = 0f;
            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            force.x -= ship.turnPower;
            targetTilt.z = maxTilt.z;
        }else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            force.x += ship.turnPower;
            targetTilt.z = -maxTilt.z;
        }else{
            targetTilt.z = 0f;
        }

        if (rb.useGravity && transform.position.y < 0f){
            rb.useGravity = false;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }

        if (Input.GetKey(KeyCode.Space)){
            if (!rb.useGravity && jumps > 0){
                jumps--;
                rb.constraints = RigidbodyConstraints.FreezeRotationY;
                rb.AddForce(0, ship.jumpPower, 0);
                rb.useGravity = true;
            }
        }
        
        rb.AddForce(force);
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, Mathf.Clamp(rb.velocity.z, 0, 24f));

    }

    void TiltPlayer(){
        if(!isAlive) return;

        Quaternion targetRotation = Quaternion.Euler(targetTilt);
        transform.localRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * ship.turnPower);
    }


    void OnTriggerEnter(Collider collider){
        if (collider.tag == "Obstacle"){
            if(isAlive){
                if(shields > 0){
                    shields--;
                    Destroy(collider.gameObject);
                }else{
                    isAlive = false;
                    gameManager.EndGame();
                }
            }
        }else{
            gameManager.HandleCapture(collider.tag);
            Destroy(collider.gameObject);
        }
    }
}

