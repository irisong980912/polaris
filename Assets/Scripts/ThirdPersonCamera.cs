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
 
    private bool _levelCleared;
    public Transform topViewCamPos;

    //InputActions
    PlayerInputActions inputAction;

    Vector2 cameraRotationInput;

    /// <summary>
    /// Camera Starting Position, creating a zoom in effect
    /// </summary>
    /// 

    void Awake()
    {
        //InputActions
        inputAction = new PlayerInputActions();
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

        } else
        {
            /*
            var xAxisInput = Input.GetAxis("Mouse X");
            var yAxisInput = Input.GetAxis("Mouse Y");
             */

            //InputAction replaces "Input.GetAxis("Example")" and calls function
            //cameraRotationInput = inputAction.Player.Look.ReadValue<Vector2>();

            inputAction.Player.Look.performed += ctx => cameraRotationInput = ctx.ReadValue<Vector2>();
            inputAction.Player.Look.canceled += ctx => cameraRotationInput = Vector2.zero;

            var xAxisInput = cameraRotationInput.x;
            var yAxisInput = cameraRotationInput.y;

            var distanceToTarget = Vector3.Distance(_mainCamera.position, _cameraTarget.position);
            if (Math.Abs(distanceToTarget - _minimumDistanceFromTarget) > 0.1f * _minimumDistanceFromTarget)
            {
                var currentCameraPosition = _mainCamera.position;
                
                var vectorToTarget = _cameraTarget.position - currentCameraPosition;
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

            //TODO: Refactor this implementation to use IsometricCamera when that feature is implemented.

            _mainCamera.LookAt(_cameraTarget); 
            
            //TODO: Add slingshot camera effects.

        }
        
    }

    private void OnOrbitStart()
    {
        Debug.Log("camera --- OnOrbitStart");
        _cameraTarget = player.parent;
        _minimumDistanceFromTarget = _distanceFromPlanet;
    }

    private void OnOrbitStop()
    {
        Debug.Log("camera --- OnOrbitStop");
        _cameraTarget = player;
        _minimumDistanceFromTarget = distanceFromPlayer;
    }
    
    private void OnLevelClear()
    {
        _levelCleared = true;
    }

    //InputActions
    //Activates all actions in action maps (action maps are Player and UI)
    //InputActions
    //Activates all actions in Player action maps (action maps are Player and UI)
    private void OnEnable()
    {
        inputAction.Player.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        inputAction.Player.Disable();
    }


}
