using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform lookAt;       // the player to lookat
    public Transform cam; // camera itself

    // set the min and max camera angle on X and Y axis
    public float Y_ANGLE_MIN = -80.0f;  // bottom degree
    public float Y_ANGLE_MAX = 80.0f; // top degree

    private float _distance = 5.0f;
    private float _currentX = 0.0f;
    private float _currentY = 0.0f;

    private void Update()
    {
        // move the camera angle by mouse
        _currentX += Input.GetAxis("Mouse X");
        _currentY += Input.GetAxis("Mouse Y");

        // unity clamp API ensures that the value is always within the range
        _currentY = Mathf.Clamp(_currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }
    private void LateUpdate()
    {
        // position the camera beind the player by "distance"
        Vector3 dir = new Vector3(0, 0, -_distance);

        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);
        // transform camera position
        cam.position = lookAt.position + rotation * dir;
        // keep the lookAt target at the center of the camera (camera follows target)
        cam.LookAt(lookAt.position);
    }

}
