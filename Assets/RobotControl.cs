using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotControl : MonoBehaviour
{
    public Rigidbody rigidBody;
    float forward, turn;



    public double target = 10;
    public float power = 1f;

    public float kP = 0.2f;
    public float kI = 0.2f;
    public float kD = 0.2f;

    public Transform left, right;




























    // Update is called once per frame ( 16 milliseconds)

    float integral = 0;
    float lastError = 0;
    float lastOutput = 0;
    void FixedUpdate()
    {





        float error = (float)target - GetPosition();

        if (Mathf.Abs(lastOutput) < 1f)
            integral += error * (Time.fixedDeltaTime);

        float derivative = (error - lastError) / (Time.fixedDeltaTime);

        float p = error * kP;
        float i = integral * kI;
        float d = derivative * kD;
        float sum = p + i + d;

        Debug.Log("P: " + p + " I: " + i + " D: " + d);

        ArcadeDrive(sum, 0);
        lastOutput = sum;
        lastError = error;




















        //Ignore this, for simulation
        HandleArcadeDrive();
    }










    float GetPosition()
    {
        return transform.position.x;
    }

    float GetHeading()
    {
        return transform.rotation.eulerAngles.y;
    }

    /** Input limited to -1 to 1
     */
    void ArcadeDrive(double forward, double turn)
    {
        this.forward = (float)forward;
        this.turn = (float)turn;
    }

    void HandleArcadeDrive()
    {
        float actualPower = forward;
        if (Mathf.Abs(actualPower) > 1f) actualPower = 1f * Mathf.Sign(actualPower);
        actualPower *= power;

        float actualTurn = turn;
        if (Mathf.Abs(actualTurn) > 1f) actualTurn = 1f * Mathf.Sign(actualTurn);
        actualTurn *= power*0.5f;

        float leftPower = actualPower + actualTurn;
        float rightPower = actualPower - actualTurn;

        rigidBody.AddForceAtPosition(new Vector3(leftPower * Time.fixedDeltaTime, 0), left.position, ForceMode.Force);
        rigidBody.AddForceAtPosition(new Vector3(rightPower * Time.fixedDeltaTime, 0), right.position, ForceMode.Force);


        //rigidBody.AddForceAtPosition(new Vector3(actualPower, 0));
        //rigidBody.AddRelativeTorque(new Vector3(0, actualTurn, 0));

        //transform.localPosition += new Vector3(forward, 0);
        //transform.localRotation *= new Quaternion(0, turn, 0, 0);
    }
}
