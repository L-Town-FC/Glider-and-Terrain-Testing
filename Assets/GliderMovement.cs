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
    static float maxRollAngle = 60;
    static float maxPitchAngle = 40;
    static float maxSpeed = 100;

    //Glider Rotation Rates
    float pitchChangeRate = 1f * maxPitchAngle;
    float rollChangeRate = 60f;
    float rollToYaw = 0.01f;
    float rollResetModifier = 1.5f;


    float stallSpeed = 6f;
    public Vector3 startingVelocity = new Vector3(0, 0, 50);
    Vector3 velocity;
    float drag = -0.005f;
    Vector3 force;

    float downForce;
    float maxDownForce = 6f;
    float downForceAccelerationRate = 8f;

    float upForce;
    float maxUpForce = 30f;
    float upForceAccelerationRate = 15f;


    bool isStalling = false;
    public bool inUpdraft = false;

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
        pitch += Input.GetAxisRaw("Vertical") * Time.fixedDeltaTime * pitchChangeRate;
        pitch = Mathf.Clamp(pitch, -maxPitchAngle, maxPitchAngle);

        horizontalInput = Input.GetAxisRaw("Horizontal");

        //Resets glider roll when no input is applied
        RollReset();

        //Sets glider rotations
        GliderRotations();

        //Setting Velocity of Glider and Applying Movement
        SettingVelocity();

        //Shows the direciton the glider is facing and the direction the glider is moving
        Debug.DrawRay(transform.position, gliderBody.forward * 10, Color.red);

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
            roll -= rollChangeRate * Mathf.Sign(roll) * rollResetModifier * Time.fixedDeltaTime;

            if (Mathf.Abs(roll) < 2f)
            {
                roll = 0f;
            }
        }
        else
        {
            roll += horizontalInput * Time.fixedDeltaTime * rollChangeRate;
            roll = Mathf.Clamp(roll, -maxRollAngle, maxRollAngle);
        }
    }

    void SettingVelocity()
    {
        UpdraftCheck();
        StallingCheck();
        GravityCheck();

        velocity.z = Mathf.Clamp(velocity.z, 0.5f, 100);

        transform.Translate(velocity * Time.fixedDeltaTime);
    }

    void StallingCheck()
    {
        //Checks if you are stalling and adjusts downward force to stimulate lack up lift on glider

        if (Mathf.Cos(pitch * Mathf.Deg2Rad) * velocity.z < stallSpeed)
        {
            isStalling = true;
            downForce += downForceAccelerationRate;
        }
        else
        {
            isStalling = false;
            downForce -= downForceAccelerationRate;
        }

        downForce = Mathf.Clamp(downForce, 0, maxDownForce);
        transform.Translate(Vector3.down * downForce * Time.fixedDeltaTime);


        if (!isStalling)
        {
            velocity.z += drag;
        }
    }

    void GravityCheck()
    {
        //checks how the glider is angled and either increases or decreases speed based on if facing upward or downward
        if (pitch > 0)
        {
            //print("down");
            force.z = pitch / 100f;
        }
        else if (pitch < 0)
        {
            //print("up");
            force.z = pitch / 100f;
        }

        velocity += force;
    }

    void UpdraftCheck()
    {
        if(inUpdraft == true)
        {
            upForce += upForceAccelerationRate;
        }
        else
        {
            upForce -= upForceAccelerationRate;
        }

        upForce = Mathf.Clamp(upForce, 0, maxUpForce);
        if((upForce * Time.fixedDeltaTime * Vector3.up).magnitude > 0.01f)
        {
            transform.Translate(upForce * Vector3.up * Time.fixedDeltaTime);
        }
    }
}
