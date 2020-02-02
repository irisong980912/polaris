using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    // The object we want to keep in frame.
    public Transform target;
    // The speed at which the camera rotates.
    public float cameraSpeed;
    // The area in which the player can move before the camera begins rotating.
    public float freeMoveRadius;


    private void Start()
    {
        transform.LookAt(target);
    }

    // LateUpdate is called once per frame, after Update()
    void LateUpdate()
    {
        var step = cameraSpeed * Time.deltaTime;
        var vectorToTarget = target.transform.position - transform.position;
        var rotationToTarget = Quaternion.LookRotation(vectorToTarget, Vector3.up);

        if (Quaternion.Angle(transform.rotation, rotationToTarget) > freeMoveRadius)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToTarget, step);
        }
        
    }
}
