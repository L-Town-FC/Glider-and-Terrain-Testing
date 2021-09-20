using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GliderMovement : MonoBehaviour
{
    Transform gliderBody;

    //Glider Rotations
    float pitch; //units are degrees
    float yaw;
    float roll;

    float horizontalInput;
    static float maxRollAngle = 45;
    static float maxPitchAngle = 40;
    static float maxSpeed = 100;

    //Glider Rotation Rates
    float pitchChangeRate = 0.5f * maxPitchAngle;
    float rollChangeRate = 30f;
    float rollToYaw = 0.01f;
    float rollResetModifier = 1.3f;

    
    float stallSpeed = 8f;
    Vector3 gravity = new Vector3(0,-0.2f,0);
    float terminalVelocity = 50f;
    float horizontalVelocityMagnitude;
    public Vector3 startingVelocity = new Vector3(0,0,50);
    Vector3 velocity;
    float drag = -0.01f;

    private void OnEnable()
    {
        gliderBody = transform.GetChild(0);
        velocity = startingVelocity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocity.z += 5f;
        }

    }



    private void FixedUpdate()
    {
        pitch += Input.GetAxisRaw("Vertical") * Time.deltaTime * pitchChangeRate;
        pitch = Mathf.Clamp(pitch, -maxPitchAngle, maxPitchAngle);

        horizontalInput = Input.GetAxisRaw("Horizontal");

        //Resets glider roll when no input is applied
        RollReset();

        //Sets glider rotations
        GliderRotations();

        //Setting Velocity of Glider and Applying Movement
        SettingVelocity();

        //Shows the direciton the glider is facing and the direction the glider is moving
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
        Debug.DrawRay(transform.position, velocity.normalized * 10, Color.blue);

    }

    void GliderRotations()
    {
        yaw += roll * rollToYaw; //turns side to side based on amount of roll applied
        transform.localEulerAngles = new Vector3(pitch, yaw, 0);
        gliderBody.localEulerAngles = new Vector3(gliderBody.localEulerAngles.x, gliderBody.localEulerAngles.y, -roll);
    }

    void RollReset()
    {
        if (horizontalInput == 0) //Sets roll to zero if close to zero and no input is applied. Otherwise it applies the input
        {
            roll -= rollChangeRate * Mathf.Sign(roll) * rollResetModifier * Time.deltaTime;

            if (Mathf.Abs(roll) < 0.8f)
            {
                roll = 0f;
            }
        }
        else
        {
            roll += horizontalInput * Time.deltaTime * rollChangeRate;
            roll = Mathf.Clamp(roll, -maxRollAngle, maxRollAngle);
        }
    }

    void SettingVelocity()
    {
        Vector3 direction;

        velocity = transform.forward * (Mathf.Clamp(velocity.magnitude + drag + Math.Sign(pitch) * Mathf.Sin(Mathf.Abs(pitch) * Mathf.Deg2Rad) * gravity.magnitude, 0, maxSpeed));
        transform.Translate(transform.InverseTransformDirection(velocity) * Time.fixedDeltaTime);
        print(velocity);
    }
}
