using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Camera camera;
    public RobotPID[] robots;
    int selectedRobotIdx = 0;
    RobotPID selectedRobot;

    Vector3[] startingPos;
    Quaternion[] startingRot;
    public Transform targetCube;

    public Vector3[] cameraPos;
    public Vector3[] cameraRot;
    int selectedCameraAngle = 0;

    /* Text and Dashboard */
    public TMPro.TextMeshProUGUI textTarget;
    public TMPro.TextMeshProUGUI textP;
    public TMPro.TextMeshProUGUI textI;
    public TMPro.TextMeshProUGUI textD;
    public TMPro.TextMeshProUGUI textSum;
    public TMPro.TextMeshProUGUI textOutput;
    public TMPro.TextMeshProUGUI textError;

    public TMPro.TMP_InputField kPInput;
    public TMPro.TMP_InputField kIInput;
    public TMPro.TMP_InputField kDInput;
    public TMPro.TMP_InputField targetInput;

    public Color errorColor = Color.red;
    public Color normalColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        camera = FindObjectOfType<Camera>();
        startingPos = new Vector3[robots.Length];
        startingRot = new Quaternion[robots.Length];
        for (int i = 0; i < robots.Length; i++)
        {
            startingPos[i] = robots[i].transform.position;
            startingRot[i] = robots[i].transform.rotation;
        }

        selectedRobotIdx = robots.Length - 1;
        selectedRobot = robots[selectedRobotIdx];
        NextRobot();

        selectedCameraAngle = cameraPos.Length - 1;
        NextCameraPos();
    }

    // Update is called once per frame
    void Update()
    {
        camera.gameObject.transform.position = selectedRobot.transform.position + cameraPos[selectedCameraAngle];

        textTarget.text = selectedRobot.GetPosition().ToString("0.00") + " -> " + selectedRobot.target.ToString("0.00");
        textError.text = "Err: " + selectedRobot.GetError().ToString("0.00");
        textP.text = "  P: " + selectedRobot.getP().ToString("0.00");
        textI.text = "  I: " + selectedRobot.getI().ToString("0.00");
        textD.text = "  D: " + selectedRobot.getD().ToString("0.00");
        textSum.text = "Sum: " + selectedRobot.getOutput().ToString("0.00");
        textOutput.text = "Output: " + selectedRobot.getActualOutput().ToString("0.00");

        Vector3 targetPos = selectedRobot.transform.position;
        targetPos[selectedRobot.axis] = (float)selectedRobot.target;
        targetCube.position = targetPos;
    }

    public void NextCameraPos()
    {
        selectedCameraAngle++;
        if (selectedCameraAngle >= cameraPos.Length) selectedCameraAngle = 0;
        camera.gameObject.transform.rotation = Quaternion.Euler(cameraRot[selectedCameraAngle]);
        camera.gameObject.transform.position = selectedRobot.transform.position + cameraPos[selectedCameraAngle];
    }

    public void NextRobot()
    {
        
        selectedRobotIdx++;
        if (selectedRobotIdx >= robots.Length) selectedRobotIdx = 0;
        selectedRobot.ResetPID();
        selectedRobot.enabled = false;
        selectedRobot = robots[selectedRobotIdx];
        selectedRobot.enabled = true;
        kPInput.text = selectedRobot.kP.ToString();
        kIInput.text = selectedRobot.kI.ToString();
        kDInput.text = selectedRobot.kD.ToString();
        targetInput.text = selectedRobot.target.ToString();
    }

    public void ResetPositions()
    {
        for (int i = 0; i < robots.Length; i++)
        {
            robots[i].transform.position = startingPos[i];
            robots[i].transform.rotation = startingRot[i];

            robots[i].ResetPID();
        }
    }

    public void SetKP(string str)
    {
        float kP;
        bool success = float.TryParse(str, out kP);
        if (!success)
        {
            kPInput.GetComponent<Image>().color = errorColor;
            return;
        }
        kPInput.GetComponent<Image>().color = normalColor;
        selectedRobot.setKP(kP);
    }
    public void SetKI(string str)
    {
        float kI;
        bool success = float.TryParse(str, out kI);
        if (!success)
        {
            kIInput.GetComponent<Image>().color = errorColor;
            return;
        }
        kIInput.GetComponent<Image>().color = normalColor;
        selectedRobot.setKI(kI);
    }

    public void SetKD(string str)
    {
        float kD;
        bool success = float.TryParse(str, out kD);
        if (!success)
        {
            kDInput.GetComponent<Image>().color = errorColor;
            return;
        }
        kDInput.GetComponent<Image>().color = normalColor;
        selectedRobot.setKD(kD);
    }

    public void SetTaget(string str)
    {
        float target;
        bool success = float.TryParse(str, out target);
        if (!success)
        {
            targetInput.GetComponent<Image>().color = errorColor;
            return;
        }
        targetInput.GetComponent<Image>().color = normalColor;
        selectedRobot.target = target;
    }

}
