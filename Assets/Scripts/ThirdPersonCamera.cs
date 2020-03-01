using System;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    private Transform _mainCamera;

    public float minimumDistanceFromTarget = 4;
    public float maximumRotationSpeed = 1.8f;
    public float cameraFollowDelay = 0.2f;

    private Transform _cameraTarget;
    private Vector3 _currentCameraVelocity = Vector3.zero;

    private bool _levelCleared;
    public Transform topViewCamPos;

    /// <summary>
    /// Camera Starting Position, creating a zoom in effect
    /// </summary>
    private void Start()
    {
        _cameraTarget = player;
        _mainCamera = transform;
        
        Orbit.OnOrbitStart += OnOrbitStart;
        Orbit.OnOrbitStop += OnOrbitStop;
        ClearLevel.OnLevelClear += OnLevelClear;

        var dir = new Vector3(0, 0, -10.0f);
        transform.position = player.position + dir;

        _mainCamera.LookAt(_cameraTarget);
    }

    private void LateUpdate()
    {
        var xAxisInput = Input.GetAxis("Mouse X");
        var yAxisInput = Input.GetAxis("Mouse Y");

        var distanceToTarget = Vector3.Distance(_mainCamera.position, _cameraTarget.position);
        if (Math.Abs(distanceToTarget - minimumDistanceFromTarget) > 0.1f * minimumDistanceFromTarget)
        {
            var currentCameraPosition = _mainCamera.position;
            
            var vectorToTarget = _cameraTarget.position - currentCameraPosition;
            var idealMovementForCamera = vectorToTarget - vectorToTarget.normalized * minimumDistanceFromTarget;
            var idealPositionForCamera = currentCameraPosition + idealMovementForCamera;

            _mainCamera.position = Vector3.SmoothDamp(
                currentCameraPosition,
                idealPositionForCamera,
                ref _currentCameraVelocity,
                cameraFollowDelay);
        }

        if (Math.Abs(xAxisInput) > 0.1f || Math.Abs(yAxisInput) > 0.1f)
        {
            distanceToTarget = Vector3.Distance(_mainCamera.position, _cameraTarget.position);
            _mainCamera.LookAt(_cameraTarget);
            
            var xRotationMagnitude = xAxisInput * maximumRotationSpeed;
            var yRotationMagnitude = yAxisInput * maximumRotationSpeed;
            
            // An upwards rotation (from the Mouse Y Axis) is a rotation about the X Axis.
            // Similarly, a sideways rotation (from Mouse X) is a rotation about the Y Axis.
            _mainCamera.Rotate(yRotationMagnitude, xRotationMagnitude, 0, Space.Self);

            // After rotating the camera, it will no longer be pointing at the player.
            // By translating the camera as follows, it will be adjusted to point at the player again.
            var lookingAtPosition = _mainCamera.position + _mainCamera.forward * distanceToTarget;
            var correctivePath = _cameraTarget.position - lookingAtPosition;
            
            _mainCamera.Translate(correctivePath, Space.World);
        }
        
        _mainCamera.LookAt(_cameraTarget);
        
        //TODO: Refactor this implementation to use IsometricCamera when that feature is implemented.
        if (!_levelCleared) return;
        var desiredPosition = topViewCamPos.position;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.0125f);

        var newRot = Quaternion.Euler(90, -45, -500); 
        transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 0.0125f);
        
        //TODO: Add slingshot camera effects.
    }

    private void OnOrbitStart()
    {
        _cameraTarget = player.parent;
        minimumDistanceFromTarget *= 2;
    }

    private void OnOrbitStop()
    {
        _cameraTarget = player;
        minimumDistanceFromTarget /= 2;
    }
    
    private void OnLevelClear()
    {
        _levelCleared = true;
    }

}
