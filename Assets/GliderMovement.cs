using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderMovement : MonoBehaviour
{
    Rigidbody rb;
    Transform gliderBody;
    Vector3 gliderRotation;
    float pitch;
    float yaw;
    float roll;
    float pitchChangeRate = 20f;
    float yawChangeRate = 20f;
    float rollChangeRate = 30f;
    float horizontalInput;
    float maxRollAngle = 45;
    float rollResetRate = 0.98f;
    float yawResetRate = 0.95f;

    private void OnEnable()
    {
        gliderBody = transform.GetChild(0);
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

    //make it so yaw is dependent on roll, not the other way around (thats how it is currently)

    private void FixedUpdate()
    {
        pitch += Input.GetAxisRaw("Vertical") * Time.deltaTime * pitchChangeRate;
        pitch = Mathf.Clamp(pitch, -30, 30);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        yaw += horizontalInput * Time.deltaTime * yawChangeRate;

        if(horizontalInput == 0)
        {
            //make roll tend towards 0 when nothing is input
            roll = roll * rollResetRate;
            if(Mathf.Abs(roll) > 10f)
            {
                yaw += Mathf.Sign(roll) * Time.deltaTime * 20f;
                print(roll);
            }
        }
        else
        {
            roll += horizontalInput * Time.deltaTime * rollChangeRate;
            roll = Mathf.Clamp(roll, -maxRollAngle, maxRollAngle);
        }


        transform.localEulerAngles = new Vector3(pitch, yaw, 0);
        gliderBody.localEulerAngles = new Vector3(gliderBody.localEulerAngles.x, gliderBody.localEulerAngles.y, -roll);
        rb.velocity = transform.TransformDirection(new Vector3(0, 0, rb.velocity.magnitude));
    }

    float Lift()
    {
        float liftForce = 0;

        return liftForce;
    }
}
