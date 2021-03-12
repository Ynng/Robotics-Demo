using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotControl : RobotPID
{
    /* User input */
    public Transform left, right;

    /*===========================================
      PID
    ===========================================*/
    void FixedUpdate()
    {
        float error = GetError();

        if (!(output > 1f && error > 0f) && !(output < -1f && error < 0f))
            integral += error * (Time.fixedDeltaTime);

        float derivative = (error - lastError) / (Time.fixedDeltaTime);
        lastError = error;

        p = error * kP;
        i = integral * kI;
        d = derivative * kD;
        output = p + i + d;

        //Debug.Log("P: " + p + " I: " + i + " D: " + d);

        Move(output, 0);

        //For simulation
        HandleMove();
    }

    /*===========================================
      SIMULATIONS
    ===========================================*/
    protected float turn;

    float GetHeading()
    {
        return transform.rotation.eulerAngles.y;
    }

    void Move(double power, double turn)
    {
        this.power = (float)power;
        this.turn = (float)turn;
    }

    void HandleMove()
    {
        float actualPower = power;
        if (Mathf.Abs(actualPower) > 1f) actualPower = 1f * Mathf.Sign(actualPower);
        actualPower *= powerMultiplier;

        float actualTurn = turn;
        if (Mathf.Abs(actualTurn) > 1f) actualTurn = 1f * Mathf.Sign(actualTurn);
        actualTurn *= powerMultiplier * 0.5f;

        float leftPower = actualPower + actualTurn;
        float rightPower = actualPower - actualTurn;

        rigidBody.AddForceAtPosition(new Vector3(leftPower * Time.fixedDeltaTime, 0), left.position, ForceMode.Force);
        rigidBody.AddForceAtPosition(new Vector3(rightPower * Time.fixedDeltaTime, 0), right.position, ForceMode.Force);
    }
}
