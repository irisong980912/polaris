using System;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    private Transform _mainCamera;

    private float _minimumDistanceFromTarget;
    public float distanceFromPlayer = 4;

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
    
    public Transform constellationViewPos;
    public Transform isometricStarViewPos;
    
    private bool _onRidePlanet;
    private bool _onIso;
    
    private Transform _curStar;
    private bool _isStarAnimating;
    private bool _finishAnimation;
    private bool _isOnPlanet;
    private bool _isPlanetAnimation;
    private bool _onSlingShot;


    //TODO: listening to create star event, when stars are created, pan the camera to 
    // show the star animation
    
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

        _cameraTarget = player;
        _mainCamera = transform;
        
        RidePlanetSlingshot.OnRidePlanet += OnRidePlanet;
        ClearLevel.OnLevelClear += OnLevelClear;
        IsometricStarPosManager.OnIsometricStarView += OnIsometricStarView;
        CreateStar.OnStarCreation += OnStarCreation;
        DestroyStar.OnStarDestruction += OnStarDestruction;
        // camera position when game starts
        _mainCamera.LookAt(_cameraTarget);
    }

    private void FixedUpdate()
    {
        
        // TODO: camera pans when stars are lit and show a small cutscene when player ride the planet 
        if (_levelCleared )
        {
            
            var desiredPosition = constellationViewPos.position ;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.0125f);
            var newRot = Quaternion.Euler(90, -45, -500); 
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 0.0125f);
        }
        else if (_enableIsometricView)
        {
            // please do not change, isometricStarViewPos position changes constantly
            // needed to be checked at every time frame
            var desiredPosition = isometricStarViewPos.position;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.0125f);
    
            var newRot = Quaternion.Euler(90, -45, -500); 
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 0.0125f);
            
            _mainCamera.LookAt(_cameraTarget);
        }
        else if (_isStarAnimating)
        {
            var dir = new Vector3(1, 0, 1);
            var desiredPosition = _curStar.position + dir * 30;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.03f);
            _mainCamera.LookAt(_curStar);
        }
        
        else if (_isPlanetAnimation && _isOnPlanet && !_onSlingShot)
        {
            if (!player.parent) return;
            var dir = new Vector3(1, 0, 1);
            var desiredPosition = player.parent.position + dir * 30;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.03f);
            // look at the planet core
            _mainCamera.LookAt(player.parent);
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

                
                // yRotationMagnitude = Mathf.Clamp(yRotationMagnitude, -80, 80);
                // xRotationMagnitude = Mathf.Clamp(xRotationMagnitude, -80, 80);
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
    
    private void OnStarCreation()
    {
        _isStarAnimating = true;
        _enableIsometricView = false; // temperorially disable 
        Invoke(nameof(FinishStarAnimation), 6.0f);
    }
    
    private void OnStarDestruction()
    {
        _isStarAnimating = true;
        _enableIsometricView = false; // temperorially disable 
        Invoke(nameof(FinishStarAnimation), 6.0f);
    }

    private void FinishStarAnimation()
    {
        _isStarAnimating = false;
        _enableIsometricView = true;
    }

    
    private void OnRidePlanet(bool isOnPlanet)
    {
        _isOnPlanet = isOnPlanet;
        
        if (isOnPlanet)
        {
            Debug.Log("camera --- OnRidePlanetStart");
            _cameraTarget = player.parent;
            _isPlanetAnimation = true;
            _enableIsometricView = false; // temperorially disable 
            Invoke(nameof(FinishPlanetAnimation), 4.0f);
        }
        else
        {
            Debug.Log("camera --- OnRidePlanetStop");
            _cameraTarget = _curStar;
            _minimumDistanceFromTarget = distanceFromPlayer;
        }
    }
    
    private void FinishPlanetAnimation()
    {
        _isPlanetAnimation = false;
        _enableIsometricView = true;
    }

    
    private void OnLevelClear()
    {
        print("clear level ------ camera !~@#@#@#@#$#$å");
        _levelCleared = true;
    }
    
    private void OnIsometricStarView(bool onIso, Transform star)
    {
        if (_levelCleared) return;
        Debug.Log("camera -- OnIsometricStarView");
        _enableIsometricView = onIso;

        _curStar = star;
        
        _cameraTarget = onIso ? star : player;
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
        RidePlanetSlingshot.OnRidePlanet -= OnRidePlanet;
        ClearLevel.OnLevelClear -= OnLevelClear;
        IsometricStarPosManager.OnIsometricStarView -= OnIsometricStarView;
    }
    

}
