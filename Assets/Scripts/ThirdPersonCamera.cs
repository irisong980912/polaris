using System;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;                      // the player to look at
    private Transform _mainCamera;                  // camera itself

    public float minimumDistanceFromTarget = 4;
    public float maximumRotationSpeed = 1.8f;
    public float cameraFollowDelay = 0.2f;

    public float smoothSpeed = 0.125f;

    private Transform _cameraTarget;
    private Vector3 _currentCameraVelocity = Vector3.zero;

    private float _currentX;
    private float _currentY;

    private bool _orbitDetected;
    private bool _firstTimeOrbit;

    private bool _levelCleared;

    private bool _onSlingShot;

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
        Orbit.OnSlingshotLaunch += OnSlingshotLaunch;
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
        if (Math.Abs(distanceToTarget - minimumDistanceFromTarget) > 1.0f)
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
        if (_levelCleared)
        {
            var desiredPosition = topViewCamPos.position;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.0125f);

            var newRot = Quaternion.Euler(90, -45, -500); 
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 0.0125f);
        } 
        else
        {
            if (!_onSlingShot) return;
            // look at the left side when player slingshots
            var desiredPosition = player.position - player.right * 10;
                
            // smooth following
            var smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothPosition;

            _mainCamera.LookAt(player);

            // TODO: still need to test this code
//             // position the camera behind the player by "distance"
//             var dir = new Vector3(0, 0, -distance);
//             var rotation = Quaternion.Euler(_currentY, _currentX, 0);
//             // smooth following
//             var desiredPosition = _target.position;
//             var smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
//             transform.position = smoothPosition + rotation * dir;
        }
        
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

    private void OnSlingshotLaunch()
    {
        _onSlingShot = true;
        Debug.Log("OnSlingShot");
        Invoke(nameof(OffSlingShot), 2);

    }

    private void OffSlingShot()
    {
        _onSlingShot = false;
        Debug.Log("OnSlingShot");
    }
    
}
