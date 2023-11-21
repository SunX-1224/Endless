using UnityEngine;

public class ParticleManager : MonoBehaviour{

    public static ParticleManager instance;

    [SerializeField] GameObject explosion;
    [SerializeField] GameObject capture;
    [SerializeField] GameObject shardCapture;
    [SerializeField] GameObject jumpParticle;

    void Awake(){
        if(instance == null) instance = this;
        else{
            Destroy(gameObject);
            return;
        }
    }

    public void Explosion(Vector3 position){
        GameObject _ex = Instantiate(explosion, position, Quaternion.identity);
        Destroy(_ex, 3f);
    }

    public void CaptureItem(Vector3 position){
        GameObject _cp = Instantiate(capture, position, Quaternion.identity);
        Destroy(_cp, 3f);
    }

    public void CaptureShard(Vector3 position){
        GameObject _scp = Instantiate(shardCapture, position, Quaternion.identity);
        Destroy(_scp, 3f);
    }

    public void JumpEffect(Transform parent){
        GameObject _j = Instantiate(jumpParticle, parent);
        Destroy(_j,3f);
    }
}
