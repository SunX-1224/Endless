using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicMovement : MonoBehaviour {
    [SerializeField] Vector3 speed;
    [SerializeField] Vector3 offset;
    [SerializeField] bool rot;
    [SerializeField] float amp;
    [SerializeField] bool abs;

    Vector3 initialPosition;
    void Start(){
        initialPosition = transform.position;
    }

    void Update(){
        if(rot) Rotate();
        else Swing();
    }

    void Rotate(){
        transform.Rotate(speed * Time.deltaTime);
    }
    
    float absSin(float x, float o){
        return Mathf.Abs(Mathf.Sin(x * Time.time + o));
    }

    void Swing(){
        Vector3 del;
        if(abs)
            del = new Vector3(absSin(speed.x, offset.x), absSin(speed.y, offset.y), absSin(speed.z, offset.z)) * amp;
        else
            del = new Vector3(Mathf.Sin(speed.x * Time.time + offset.x), Mathf.Sin(speed.y * Time.time  +offset.y), Mathf.Sin(speed.z * Time.time + offset.z)) * amp;

        transform.position  = initialPosition + del;
    }
}
