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
    
    Vector3 maxTilt = new Vector3(20,0, 50);
    Rigidbody rb;
    Coroutine tiltCoroutine;
    Ship ship;

    public float minVelocity;

    void Start(){
        gameManager = GetComponentInParent<GameManager>();
        ship = GetComponent<Ship>();
        rb = GetComponent<Rigidbody>();
        
        minVelocity = 18f;
        rb.velocity = new(0,0,ship.velocity);
    }

    void Update(){
        if(!isAlive) return;
        TiltPlayer();
    }

    void FixedUpdate(){
        if(!isAlive) return;
        HandleControls();
    }

    public float GetForce(){
        return ship.boost; 
    }

    public float GetTurnPower(){
        return ship.turnVelocity;
    }

    void HandleControls(){
        if(!isAlive) return;

        Vector3 force = new(0, 0, 0);
        float vx = rb.velocity.x;

        targetTilt.y = 0;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        force.z = ship.boost * v;
        targetTilt.x = Mathf.Min(maxTilt.x * v, 0f);
        vx = ship.turnVelocity * h;
        targetTilt.z = -maxTilt.z * h;

        boostActive = force.z > 0f;

        if (rb.useGravity && transform.position.y < 0f){
            rb.useGravity = false;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }

        if (Input.GetKey(KeyCode.Space)){
            if (!rb.useGravity && jumps > 0){
                jumps--;
                rb.constraints = RigidbodyConstraints.None;
                rb.AddForce(0, ship.jumpForce, 0);
                rb.useGravity = true;
            }
        }
        
        rb.AddForce(force);
        rb.velocity = new(vx, rb.velocity.y, Mathf.Clamp(rb.velocity.z, minVelocity, 36f)); 
    }

    void TiltPlayer(){
            
        Quaternion targetRotation = Quaternion.Euler(targetTilt);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, ship.turnVelocity);            
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

