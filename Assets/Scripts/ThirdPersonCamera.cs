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
    
    public float maximumRotationSpeed = 1.8f;
    public float cameraFollowDelay = 0.2f;

    private Transform _cameraTarget;
    private Vector3 _currentCameraVelocity = Vector3.zero;

    //InputActions
    private PlayerInputActions _inputAction;
    private readonly Vector2 _cameraRotationInput;


    Vector2 cameraRotationInput;

    private bool _enableIsometricView;
    private bool _levelCleared;

    private Transform _viewPos;
    public Transform constellationViewPos;
    public Transform isometricStarViewPos;
    
    private bool _onOrbit;
    private bool _onIso;

    public Transform firstStar;

    /// <summary>
    /// Camera Starting Position, creating a zoom in effect
    /// </summary>
    /// 


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
        // TODO: change to focus on planet in the same angle
        _minimumDistanceFromTarget = distanceFromPlayer;
        _distanceFromPlanet = distanceFromPlayer * onOrbitDistanceRatio;
            
        _cameraTarget = player;
        _mainCamera = transform;
        
        Orbit.OnOrbitStart += OnOrbitStart;
        Orbit.OnOrbitStop += OnOrbitStop;
        ClearLevel.OnLevelClear += OnLevelClear;
        IsometricStarPosManager.OnIsometricStarView += OnIsometricStarView;

        // camera position when game starts
        _mainCamera.LookAt(_cameraTarget);
    }

    private void FixedUpdate()
    {
        
        // TODO: camera pans when stars are lit and show a small cutscene when player ride the planet 
        if ((_levelCleared || _enableIsometricView))
        {
            
            var desiredPosition = _viewPos.position ;
            
            // print("desired position -- camera  " + desiredPosition);
            // print("iso position -- camera  " + isometricStarViewPos.position);
            //
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
        _onOrbit = true;
        Debug.Log("camera --- OnOrbitStart");

        
        _cameraTarget = player.parent;
        _minimumDistanceFromTarget = _distanceFromPlanet;
    }

    private void OnOrbitStop()
    {
        _onOrbit = false;
        Debug.Log("camera --- OnOrbitStop");
        _cameraTarget = player;
        _minimumDistanceFromTarget = distanceFromPlayer;
    }
    
    private void OnLevelClear()
    {
        print("clear level ------ camera !~@#@#@#@#$#$å");
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
        Debug.Log("camera -- OnIsometricStarView");
        _enableIsometricView = onIso;
        
        if (onIso)
        {
            _viewPos = isometricStarViewPos;
            _cameraTarget = star;
        }

        else
        {
            _cameraTarget = player;
        }
    }

}
