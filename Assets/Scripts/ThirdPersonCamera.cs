﻿using System;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    private Transform _mainCamera;

    private float _minimumDistanceFromTarget;
    public float distanceFromPlayer = 4;
    private float _distanceFromPlanet;
    public float onOrbitDistanceRatio = 8.0f;
    
    public float maximumRotationSpeed = 1.8f;
    public float cameraFollowDelay = 0.2f;

    private Transform _cameraTarget;
    private Vector3 _currentCameraVelocity = Vector3.zero;
 
    private bool _levelCleared;
    public Transform topViewCamPos;

    //InputActions
    private PlayerInputActions _inputAction;
    private readonly Vector2 _cameraRotationInput;

    public ThirdPersonCamera(Vector2 cameraRotationInput)
    {
        _cameraRotationInput = cameraRotationInput;
    }

    private void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();
    }

    private void Start()
    {
        _minimumDistanceFromTarget = distanceFromPlayer;
        _distanceFromPlanet = distanceFromPlayer * onOrbitDistanceRatio;
            
        _cameraTarget = player;
        _mainCamera = transform;
        
        Orbit.OnOrbitStart += OnOrbitStart;
        Orbit.OnOrbitStop += OnOrbitStop;
        ClearLevel.OnLevelClear += OnLevelClear;

        var dir = new Vector3(0, 0, -10.0f);
        transform.position = player.position + dir;

        _mainCamera.LookAt(_cameraTarget);
    }

    private void FixedUpdate()
    {

        if (_levelCleared)
        {
            var desiredPosition = topViewCamPos.position;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.0125f);

            var newRot = Quaternion.Euler(90, -45, -500); 
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 0.0125f);

        } 
        else
        {
            // TODO: make the camera rotate faster to follow player rotation

            var xAxisInput = _cameraRotationInput.x;
            var yAxisInput = _cameraRotationInput.y;

            var distanceToTarget = Vector3.Distance(_mainCamera.position, _cameraTarget.position);
            if (Math.Abs(distanceToTarget - _minimumDistanceFromTarget) > 0.1f * _minimumDistanceFromTarget)
            {
                var currentCameraPosition = _mainCamera.position;
                
                var vectorToTarget = _cameraTarget.position - _cameraTarget.forward - currentCameraPosition;
                var idealMovementForCamera = vectorToTarget - vectorToTarget.normalized * _minimumDistanceFromTarget;
                var idealPositionForCamera = currentCameraPosition + idealMovementForCamera;
                
                _mainCamera.position = Vector3.SmoothDamp(
                    currentCameraPosition,
                    idealPositionForCamera,
                    ref _currentCameraVelocity,
                    cameraFollowDelay);
            }
            
            if (Math.Abs(xAxisInput) > 0.1f || Math.Abs(yAxisInput) > 0.1f)
            {
                var mainCameraPos = _mainCamera.position;
                var cameraTargetPos = _cameraTarget.position;
                distanceToTarget = Vector3.Distance(mainCameraPos, cameraTargetPos);
                _mainCamera.LookAt(_cameraTarget);
                
                var xRotationMagnitude = xAxisInput * maximumRotationSpeed;
                var yRotationMagnitude = yAxisInput * maximumRotationSpeed;
                
                // An upwards rotation (from the Mouse Y Axis) is a rotation about the X Axis.
                // Similarly, a sideways rotation (from Mouse X) is a rotation about the Y Axis.
                _mainCamera.Rotate(yRotationMagnitude, xRotationMagnitude, 0, Space.Self);
            
                // After rotating the camera, it will no longer be pointing at the player.
                // By translating the camera as follows, it will be adjusted to point at the player again.
                var lookingAtPosition = mainCameraPos + _mainCamera.forward * distanceToTarget;
                var correctivePath = cameraTargetPos - lookingAtPosition;
                
                _mainCamera.Translate(correctivePath, Space.World);
            }
            
            _mainCamera.LookAt(_cameraTarget);
        }
        
    }

    private void OnOrbitStart()
    {
        _cameraTarget = player.parent;
        _minimumDistanceFromTarget = _distanceFromPlanet;
    }

    private void OnOrbitStop()
    {
        _cameraTarget = player;
        _minimumDistanceFromTarget = distanceFromPlayer;
    }
    
    private void OnLevelClear()
    {
        _levelCleared = true;
    }

    //InputActions
    //Activates all actions in action maps (action maps are Player and UI)
    private void OnEnable()
    {
        _inputAction.Player.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        _inputAction.Player.Disable();
    }


}
