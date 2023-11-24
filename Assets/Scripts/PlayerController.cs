using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct MeshData{
    public Mesh mesh;
    public MeshRenderer meshRenderer;
};

public class PlayerController: MonoBehaviour{

    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public int jumps;
    [HideInInspector] public int shields;
    [HideInInspector] public bool boostActive;
    [HideInInspector] public Vector3 targetTilt;
    [HideInInspector] public Vector3 velocity;
    [HideInInspector] public float turnVelocity;

    [SerializeField] float minVelocity;
    [SerializeField] float maxVelocity;
    [SerializeField] float boost;
    [SerializeField] float turnPower;
    [SerializeField] float jumpPower;
    [SerializeField] GameManager gameManager;
    [SerializeField] Button jumpBtn;
    [SerializeField] TMP_Text jumpsCountUI;
    [SerializeField] TMP_Text shieldsCountUI;
    [SerializeField] List<MeshData> shipMeshes;

    Vector3 maxTilt = new Vector3(20,0, 50);
    Rigidbody rb;

    void Awake(){
        int shipIndex = PlayerInfo.GetShipIndex();
        
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.sharedMaterials = shipMeshes[shipIndex].meshRenderer.sharedMaterials;
        GetComponent<MeshFilter>().mesh = shipMeshes[shipIndex].mesh;

        rb = GetComponent<Rigidbody>();
        PickUpsUIUpdate();
    }

    void Update(){
        if(!isAlive) return;
        TiltPlayer();
    }

    void FixedUpdate(){
        if(!isAlive) return;
        HandleControls();
    }

    public void HandleTransition(){
        transform.position = Vector3.zero;
        minVelocity += 2f;
        rb.velocity = new(0f, 0f, minVelocity);
    }

    void HandleControls(){

        Vector3 force = new(0, 0, 0);
        float vx = rb.velocity.x;

        targetTilt.y = 0;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        force.z = boost * v;
        targetTilt.x = Mathf.Min(maxTilt.x * v, 0f);
        vx = turnPower * h;
        targetTilt.z = -maxTilt.z * h;

        boostActive = force.z > 0f;

        if (rb.useGravity && transform.position.y < 0f){
            rb.useGravity = false;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }

        if (Input.GetKey(KeyCode.Space)){
            HandleJump();
        }
        
        rb.AddForce(force);
        rb.velocity = new(vx, rb.velocity.y, Mathf.Clamp(rb.velocity.z, minVelocity, maxVelocity + (boostActive?3f:0f))); 

        velocity =rb.velocity;    
    }

    public void HandleJump(){
        if (!rb.useGravity && jumps > 0){
            jumps--;
            PickUpsUIUpdate();
            PushUp();
            ParticleManager.instance.JumpEffect(transform);
        }
    }

    void PickUpsUIUpdate(){
        jumpBtn.interactable = jumps > 0;
        jumpsCountUI.text = jumps.ToString();
        shieldsCountUI.text = shields.ToString();
    }

    void PushUp(){
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(0, jumpPower, 0);
        AudioManager.instance.PlaySFX("jump");
        rb.useGravity = true;
    }

    void TiltPlayer(){
            
        Quaternion targetRotation = Quaternion.Euler(targetTilt);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnPower);            
    }


    void OnTriggerEnter(Collider collider){
        if(!isAlive) return;
        
        if (collider.tag == "Obstacle"){
            if(isAlive){
                if(shields > 0){
                    shields--;
                    PickUpsUIUpdate();
                    PushUp();
                    AudioManager.instance.PlaySFX("shield");
                    ParticleManager.instance.JumpEffect(transform);
                    ParticleManager.instance.Explosion(transform.position + new Vector3(0f, -1f, 2f));
                    Destroy(collider.gameObject);
                }else{
                    isAlive = false;
                    rb.velocity = Vector3.zero;
                    AudioManager.instance.PlaySFX("death");
                    gameManager.EndGame();
                }
            }
        }else{
            HandleCapture(collider.tag);
            Destroy(collider.gameObject);
        }
    }
    
    public void HandleCapture(string tag){
        if (tag == "Shard"){
            gameManager.AddScore(10);
            gameManager.AddShards(1);
            ParticleManager.instance.CaptureShard(transform.position);
            AudioManager.instance.PlaySFX("shardcap");
        }
        else if (tag == "Jump"){
            jumps++;
            PickUpsUIUpdate();
            ParticleManager.instance.CaptureItem(transform.position);
            AudioManager.instance.PlaySFX("capture");
        }
        else if (tag == "Shield"){
            shields++;
            PickUpsUIUpdate();
            ParticleManager.instance.CaptureItem(transform.position);
            AudioManager.instance.PlaySFX("capture");
        }
    }
}

