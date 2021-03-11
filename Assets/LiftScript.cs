﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftScript : MonoBehaviour
{
    public Rigidbody rigidBody;
    float forward;



    public double target = 10;
    public float power = 1f;

    public float kP = 0.2f;
    public float kI = 0.2f;
    public float kD = 0.2f;




























    // Update is called once per frame ( 16 milliseconds)

    float integral = 0;
    float lastError = 0;
    float lastOutput = 0;
    void FixedUpdate()
    {
        float error = (float)target - GetPosition();

        if (!(lastOutput > 1f && error > 0f) && !(lastOutput < -1f && error < 0f))
            integral += error * (Time.fixedDeltaTime);

        float derivative = (error - lastError) / (Time.fixedDeltaTime);

        float p = error * kP;
        float i = integral * kI;
        float d = derivative * kD;
        float sum = p + i + d;

        Debug.Log("P: " + p + " I: " + i + " D: " + d);

        Move(sum);
        lastOutput = sum;
        lastError = error;




















        //Ignore this, for simulation
        HandleMove();
    }










    float GetPosition()
    {
        return transform.position.y;
    }

    /** Input limited to -1 to 1
     */
    void Move(double forward)
    {
        this.forward = (float)forward;
    }

    void HandleMove()
    {
        float actualPower = forward;
        if (Mathf.Abs(actualPower) > 1f) actualPower = 1f * Mathf.Sign(actualPower);
        actualPower *= power;

        rigidBody.AddForce(new Vector3(0, actualPower * Time.fixedDeltaTime, 0),ForceMode.Force);

        //rigidBody.AddForceAtPosition(new Vector3(actualPower, 0));
        //rigidBody.AddRelativeTorque(new Vector3(0, actualTurn, 0));

        //transform.localPosition += new Vector3(forward, 0);
        //transform.localRotation *= new Quaternion(0, turn, 0, 0);
    }
}
