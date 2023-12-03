using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct MeshData{
    public Mesh mesh;
    public MeshRenderer meshRenderer;
};

public class PlayerController: MonoBehaviour{

    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public bool revived = false;
    [HideInInspector] public int jumps;
    [HideInInspector] public int shields;
    [HideInInspector] public Vector3 targetTilt;
    [HideInInspector] public float turnVelocity;
    [HideInInspector] public Rigidbody rb;

    public float minVelocity;
    public float maxVelocity;
    public float boost;
    public float turnPower;
    public float jumpPower;

    [SerializeField] GameManager gameManager;
    [SerializeField] Button jumpBtn;
    [SerializeField] TMP_Text jumpsCountUI;
    [SerializeField] TMP_Text shieldsCountUI;
    [SerializeField] List<MeshData> shipMeshes;


    [SerializeField] Camera cam;

    Vector3 maxTilt = new Vector3(20,0, 50);
    int vertInput = 0;

    void Awake(){
        int shipIndex = PlayerInfo.GetShipIndex();
        
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.sharedMaterials = shipMeshes[shipIndex].meshRenderer.sharedMaterials;
        GetComponent<MeshFilter>().mesh = shipMeshes[shipIndex].mesh;

        rb = GetComponent<Rigidbody>();
        PickUpsUIUpdate();
    }

    void Update(){
        if(!isAlive){
            rb.velocity = Vector3.zero;
            return;
        }
        TiltPlayer();
    }

    void FixedUpdate(){
        if(!isAlive) return;
        HandleControls();
    }

    public void HandleTransition(){
        transform.position = Vector3.zero;
        minVelocity += 2f;
        maxVelocity += 2f;
        rb.velocity = new(0f, 0f, minVelocity);
    }

    void HandleControls(){

        Vector3 force = new(0, 0, 0.07f);

        targetTilt.y = 0;
        
        float v = (float) vertInput;
        HandleTouch(out float h);

        force.z = boost * v;
        targetTilt.x = Mathf.Min(maxTilt.x * v, 0f);
        targetTilt.z = -maxTilt.z * h;

        if (rb.useGravity && transform.position.y < -0.2f){
            rb.useGravity = false;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }

        if (Input.GetKey(KeyCode.Space)){
            HandleJump();
        }
        
        rb.AddForce(force);
        rb.velocity = new(rb.velocity.z * h, rb.velocity.y, Mathf.Clamp(rb.velocity.z, minVelocity, maxVelocity));
        targetTilt.x -= rb.velocity.y * 2f;
    }

    public void VerticalInput(int dir){
        vertInput = dir;
    }

    public void ResetVerticalInput(){
        vertInput = 0;
    }

    void HandleTouch(out float h){
        h = 0f;
        foreach (Touch touch in Input.touches){
            if(EventSystem.current.IsPointerOverGameObject(touch.fingerId)) continue;
            float x = (touch.position.x / Screen.width - 0.5f)*1.8f;
            h = Mathf.Abs(x) > Mathf.Abs(h)?x:h;
        }
    }

    public void HandleJump(){
        if (!rb.useGravity && jumps > 0){
            PushUp();
            jumps--;
            PickUpsUIUpdate();
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
        rb.useGravity = true;
        AudioManager.instance.PlaySFX("jump");
    }

    void TiltPlayer(){
        Quaternion targetRotation = Quaternion.Euler(targetTilt);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3f * Time.deltaTime);
    }

    public void CrashSurvival(){
        PushUp();
        AudioManager.instance.PlaySFX("shield");
        ParticleManager.instance.JumpEffect(transform);
    }

    void OnTriggerEnter(Collider collider){
        if(!isAlive) return;
        
        if (collider.tag == "Obstacle"){
            Destroy(collider.gameObject);
            ParticleManager.instance.Explosion(transform.position + new Vector3(0f, -1f, 5f));
            if(shields > 0){
                shields--;
                PickUpsUIUpdate();
                CrashSurvival();
            }else{
                gameManager.HandlePlayerCrash();
            }
        }else{
            HandleCapture(collider.tag);
            Destroy(collider.gameObject);
        }
    }
    
    public void HandleCapture(string tag){
        if (tag == "Shard"){
            gameManager.score += 10;
            gameManager.shards += 1;
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

