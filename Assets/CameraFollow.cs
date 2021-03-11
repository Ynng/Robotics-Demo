using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    public Vector3 offset;
    public Camera camera;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        camera.gameObject.transform.position = transform.position + offset;
    }
}
