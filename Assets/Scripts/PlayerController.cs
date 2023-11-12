using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Player{

    public bool isAlive;
    public float thrust;
    public float boost;
    public bool boostActive;

    public float jumpPower;
    public float turnPower;

    public Vector3 maxTilt;
    public Vector3 targetTilt;
}

public class PlayerController : MonoBehaviour{
    public Player player;

    private Rigidbody rb;
    private Coroutine tiltCoroutine;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    void Update(){
        TiltPlayer(player.targetTilt, player.turnPower);
    }

    void FixedUpdate(){
        HandleControls();
    }

    void HandleControls(){
        if(!player.isAlive) return;

        Vector3 force = new(0, 0, 0);
        force.z = player.thrust;
        player.targetTilt.y = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
            force.z += player.boost;
            player.boostActive = true;
        }else{
            player.boostActive = false;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
                force.z -= player.boost;
                player.targetTilt.x = -player.maxTilt.x;
            }else{
                player.targetTilt.x = 0f;
            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            force.x -= player.turnPower;
            player.targetTilt.z = player.maxTilt.z;
        }else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            force.x += player.turnPower;
            player.targetTilt.z = -player.maxTilt.z;
        }else{
            player.targetTilt.z = 0f;
        }

        if (rb.useGravity && transform.position.y < 0f){
            rb.useGravity = false;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }

        if (Input.GetKey(KeyCode.Space)){
            if (!rb.useGravity){
                rb.constraints = RigidbodyConstraints.FreezeRotationY;
                rb.AddForce(0, player.jumpPower, 0);
                rb.useGravity = true;
            }
        }
        
        rb.AddForce(force);
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, Mathf.Clamp(rb.velocity.z, 0, 24f));

    }

    void TiltPlayer(Vector3 targetTilt, float speed){
        if(!player.isAlive) return;

        Quaternion targetRotation = Quaternion.Euler(targetTilt);
        transform.localRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }

    void OnCollisionEnter(Collision collision){
        // Update this after using 3d models as obstacles
        if (collision.collider.tag == "Obstacle"){
            if(player.isAlive){
                player.isAlive = false;
                FindObjectOfType<GameManager>().EndGame();
            }
        }
    }
}

