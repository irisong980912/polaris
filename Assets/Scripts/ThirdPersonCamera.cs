using System;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    private Transform _mainCamera;

    private float _minimumDistanceFromTarget;
    public float distanceFromPlayer = 4;
    private float _distanceFromPlanet;
    public float onOrbitDistanceRatio = 8.0f;
    
    public float cameraFollowDelay = 0.2f;

    private Transform _cameraTarget;
    private Vector3 _currentCameraVelocity = Vector3.zero;

    //InputActions
    private PlayerInputActions _inputAction;

    private bool _enableIsometricView;
    private bool _levelCleared;

    private Transform _viewPos;
    public Transform constellationViewPos;
    public Transform isometricStarViewPos;
    
    private bool _onIso;

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
        IsometricStarPosManager.OnIsometricStarView += OnIsometricStarView;

        var dir = new Vector3(-10.0f, 0, 0f);
        transform.position = player.position + dir;

        _mainCamera.LookAt(_cameraTarget);
    }

    private void FixedUpdate()
    {
        if (_levelCleared || _enableIsometricView)
        {
            var desiredPosition = _viewPos.position ;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.0125f);

            var newRot = Quaternion.Euler(90, -45, -500); 
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 0.0125f);
        }
        
        if (_enableIsometricView)
        {
            _mainCamera.LookAt(_cameraTarget);
        }

        else
        {
            // TODO: make the camera rotate faster to follow player rotation

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
        _viewPos = constellationViewPos;
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
        //Prevent event from looking for prescribed object that is removed on Reload of scene, by unsubscribing.
        Orbit.OnOrbitStart -= OnOrbitStart;
        Orbit.OnOrbitStop -= OnOrbitStop;
        ClearLevel.OnLevelClear -= OnLevelClear;
        IsometricStarPosManager.OnIsometricStarView -= OnIsometricStarView;
    }
    
    private void OnIsometricStarView(bool onIso, Transform star)
    {
        if (_levelCleared) return;
        _onIso = onIso;

        if (_onIso)
        {
            _viewPos = isometricStarViewPos;
            _enableIsometricView = true;
            _cameraTarget = star;
        }

        else
        {
            _enableIsometricView = false;
            _cameraTarget = player;
        }
        
    }

}
