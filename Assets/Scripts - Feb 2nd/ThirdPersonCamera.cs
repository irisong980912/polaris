﻿using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform character;          // the player to look at
    public Transform cam;             // camera itself

    // set the min and max camera angle on X and Y axis
    public float yAngleMin = -80.0f;  // bottom degree
    public float yAngleMax = 80.0f;   // top degree
    public float smoothSpeed = 0.125f;
    public float distance = 0.8f;

    private float _currentX;
    private float _currentY;

    private Transform _target = null;

    private bool _isTriggered = false;
    private bool _isCancel = false;

    public Vector3 offset;

    /// <summary>
    /// Camera Starting Position, creating a zoom in effect
    /// </summary>
    private void Start()
    {
        _target = character;

        var dir = new Vector3(0, 0, -10.0f);
        var rotation = Quaternion.Euler(_currentY, _currentX, 0);
        transform.position = character.position + rotation * dir;
    }


    private void Update()
    {
        // move the camera angle by mouse
        _currentX += Input.GetAxis("Mouse X");
        _currentY += Input.GetAxis("Mouse Y");

        // unity clamp API ensures that the value is always within the range
        _currentY = Mathf.Clamp(_currentY, yAngleMin, yAngleMax);
    }


    private void LateUpdate()
    {

        if (_isTriggered)
        {
            smoothSpeed = 0.08f;
            _isTriggered = false;

        }
        else if (_isCancel)
        {
            // set back to default
            smoothSpeed = 0.125f;
            _isCancel = false;

        }

        // position the camera behind the player by "distance"
        var dir = new Vector3(0, 0, -distance);
        var rotation = Quaternion.Euler(_currentY, _currentX, 0);
        // smooth following
        Vector3 desiredPosition = _target.position;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothPosition + rotation * dir;

        cam.LookAt(_target);
    }


    public void OrbitDetected(Transform star)
    {
        _target = star;
        _isTriggered = true;
        Debug.Log("OrbitDetected");
    }


    public void CancelFocus()
    {
        _target = character;
        _isCancel = true;
        Debug.Log("CancelFocus");
    }


}
