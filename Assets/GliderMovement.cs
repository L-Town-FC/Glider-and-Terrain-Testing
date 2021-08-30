using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector3 gliderRotation;
    float pitch;
    float yaw;
    float roll;
    float pitchChangeRate = 20f;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, 50);
    }


    // Start is called before the first frame update
    void Start()
    {
        gliderRotation = transform.localEulerAngles;
        pitch = gliderRotation.x;
        yaw = gliderRotation.y;
        roll = gliderRotation.z;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        pitch += Input.GetAxisRaw("Vertical") * Time.deltaTime * pitchChangeRate;
        pitch = Mathf.Clamp(pitch, -30, 30);
        print(pitch);
        transform.localEulerAngles = new Vector3(pitch, 0, 0);
        rb.velocity = transform.TransformDirection(new Vector3(0, 0, rb.velocity.z));
    }

    float Lift()
    {
        float liftForce = 0;

        return liftForce;
    }
}
