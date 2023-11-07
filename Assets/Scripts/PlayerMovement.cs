using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    Camera cam;

    float initialFOV;
    float pushedFOV;

    public float thrust = 3.0f;
    public float boost = 1.0f;
    public float turnPower = 5.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = transform.Find("Camera").GetComponent<Camera>();
        initialFOV = cam.fieldOfView;
        pushedFOV = initialFOV + 10f;
    }

    void Update()
    {
        Vector3 force = thrust * Vector3.forward;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            force += Vector3.forward * boost;
            cam.fieldOfView = pushedFOV;
        }
        else
        {
            cam.fieldOfView = initialFOV;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            force += Vector3.left * turnPower;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            force += Vector3.right * turnPower;

        rb.AddForce(force);
    }


}
