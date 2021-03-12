using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPID : MonoBehaviour
{
    /* User input */
    public float powerMultiplier;
    public double target = 10;
    public int axis; //The axis the robot moves on

    /* Weights */
    public float kP = 0.5f;
    public float kI = 0.01f;
    public float kD = 0.01f;

    /* For PID calculations */
    protected float integral = 0;
    protected float lastError = 0;

    /* PID outputs */
    protected float p, i, d;
    protected float output = 0f;

    /* Other */
    protected Rigidbody rigidBody;

    private void Start()
    {
        ResetPID();
        if(!rigidBody)
            rigidBody = GetComponent<Rigidbody>();
    }

    /*===========================================
      PID
    ===========================================*/
    void FixedUpdate()
    {
        float error = GetError();

        //Don't accumulate integral in ways that causes the output to overflow
        if (!(output > 1f && error > 0f) && !(output < -1f && error < 0f))
            integral += error * (Time.fixedDeltaTime);

        float derivative = (error - lastError) / (Time.fixedDeltaTime);
        lastError = error;

        p = error * kP;
        i = integral * kI;
        d = derivative * kD;
        output = p + i + d;

        //Debug.Log("P: " + p + " I: " + i + " D: " + d);

        Move(output);

        //For simulation
        HandleMove();
    }

    /*===========================================
      SIMULATIONS
    ===========================================*/

    protected float power;
    void Move(double power)
    {
        this.power = (float)power;
    }

    void HandleMove()
    {
        float actualPower = power;
        if (Mathf.Abs(actualPower) > 1f) actualPower = 1f * Mathf.Sign(actualPower);
        actualPower *= powerMultiplier;

        Vector3 force = Vector3.zero;
        force[axis] = actualPower * Time.fixedDeltaTime;

        rigidBody.AddForce(force, ForceMode.Force);
    }

    public void ResetPID()
    {
        integral = 0;
        lastError = (float)target - GetPosition();
    }

    public float GetError()
    {
        return (float)target - GetPosition();
    }

    /*===========================================
      GETTERS
    ===========================================*/

    public float getP()
    {
        return p;
    }
    public float getI()
    {
        return i;
    }
    public float getD()
    {
        return d;
    }
    public float getOutput()
    {
        return output;
    }
    public float getActualOutput()
    {
        float actualPower = power;
        if (Mathf.Abs(actualPower) > 1f) actualPower = 1f * Mathf.Sign(actualPower);
        return actualPower;
    }
    public float GetPosition()
    {
        return transform.position[axis];
    }

    /*===========================================
      SETTERS
    ===========================================*/
    public void setKP(float kP)
    {
        this.kP = kP;
    }
    public void setKI(float kI)
    {
        this.kI = kI;
    }
    public void setKD(float kD)
    {
        this.kD = kD;
    }
}
