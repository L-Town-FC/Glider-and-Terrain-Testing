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
    float rollChangeRate = 30f;
    float horizontalInput;
    float maxRollAngle = 45;
    float rollToYaw = 0.01f;
    float rollResetModifier = 1.3f;
    float minSpeed = 8f;
    float gravity = 3f;
    float terminalVelocity = 50f;
    

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

    //make it so gravity is dependent on angle and horizontal velocity(not true velocity that has vertical component)
    //gravity opposing lift force. Lift force decreases as angle moves away from 0

    private void FixedUpdate()
    {
        pitch += Input.GetAxisRaw("Vertical") * Time.deltaTime * pitchChangeRate;
        pitch = Mathf.Clamp(pitch, -30, 30);

        horizontalInput = Input.GetAxisRaw("Horizontal");

        if(horizontalInput == 0)
        {
            if(Mathf.Abs(roll) <= 0.1)
            {
                roll = 0f;
            }
            if(Mathf.Sign(roll) == -1)
            {
                roll += rollChangeRate * Time.deltaTime * rollResetModifier;
                if(Mathf.Sign(roll) == 1)
                {
                    roll = 0f;
                }

            }
            else
            {
                roll -= rollChangeRate * Time.deltaTime * rollResetModifier;
                if (Mathf.Sign(roll) == -1)
                {
                    roll = 0f;
                }
            }
        }
        else
        {
            roll += horizontalInput * Time.deltaTime * rollChangeRate;
            roll = Mathf.Clamp(roll, -maxRollAngle, maxRollAngle);
        }

        //print(transform.InverseTransformDirection(rb.velocity));

        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
        

        yaw += roll * rollToYaw;


        transform.localEulerAngles = new Vector3(pitch, yaw, 0);
        gliderBody.localEulerAngles = new Vector3(gliderBody.localEulerAngles.x, gliderBody.localEulerAngles.y, -roll);
        //rb.velocity = transform.TransformDirection(new Vector3(0, 0, rb.velocity.magnitude));
        if (rb.velocity.z <= minSpeed) //checks if glider is stalling out from going to slow //CHange so it is sqrt(z^2 *x^2)
        {
            rb.velocity -= new Vector3(0, gravity * Time.deltaTime, 0); //applies a downward force if stalling out
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -terminalVelocity, terminalVelocity), rb.velocity.z); //clamps the y velocity so you can't infinitely accelerate downwards by stalling. May have to add same thing for just going downwards
        }
        else
        {
            rb.velocity = transform.TransformDirection(new Vector3(0, 0, rb.velocity.magnitude));
        }
    }

    float Lift()
    {
        float liftForce = 0;

        return liftForce;
    }
}
