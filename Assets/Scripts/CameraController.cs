using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public float smoothSpeed;


    // Update is called once per frame
    void LateUpdate()
    {

        // // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(target);
        

    }
}
