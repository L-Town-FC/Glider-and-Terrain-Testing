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
    float yawChangeRate = 20f;
    float horizontalInput;
    float maxRollAngle = 45;
    float rollResetRate = 0.5f;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, 50);
    }


    // Start is called before the first frame update
    void Start()
    {
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

        horizontalInput += Input.GetAxisRaw("Horizontal");
        yaw += horizontalInput * Time.deltaTime * yawChangeRate;

        if(horizontalInput == 0 && roll != 0)
        {
            //make roll tend towards 0 when nothing is input
            roll = roll * rollResetRate;
        }
        else
        {
            roll += horizontalInput * Time.deltaTime * rollResetRate;
            roll = Mathf.Clamp(roll, -maxRollAngle, maxRollAngle);
        }

        transform.localEulerAngles = new Vector3(pitch, yaw, 0);
        rb.velocity = transform.TransformDirection(new Vector3(0, 0, rb.velocity.magnitude));
    }

    float Lift()
    {
        float liftForce = 0;

        return liftForce;
    }
}
