using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;

/*
 * This is a long class, violates SRP! Currently, it handles:
 *  - interpretation of movement inputs
 *  - slingshot mechanic
 *  - the collection of stardust
 */

// Use the strategy interface (design pattern) to handle movement.

public class ThirdPersonPlayer : MonoBehaviour

{
    public float speed;
    public float maximumTurnRate;
    
    //InputActions
    private PlayerInputActions _inputAction;
    
    //Movement
    private Vector2 _movementInput;
    private bool _enableIsoViewMovement;

    public int stardust;
    public TextMeshProUGUI stardustCount;
    public List<GameObject> inventory = new List<GameObject>();
    
    public AudioSource collectDustSound;
    public GameObject collectDustSoundContainer;

    private static bool _mapActive;

    private bool _onSlingShot;
    private Transform _starToGo;
    public Vector3 disFromGoalStar = new Vector3(1, 0,1);

    private bool _beginSlingshot;
    public static event Action<bool> OnEndSlingShot;

    public Vector3 playerIsoEnterPos;
    private bool _firstTimeEnterIso;
    public Vector3 playerIsoExitPos;
    private bool _firstTimeExitIso;

    public Transform playerIcon;

    private Transform _curStar;

    // player need to face the first star when game started
    public Transform firstStar;
    private bool _isExitPlanetOrbit;
    private Vector3 _exitPlanetOrbitPos;

    private void Start()
    {
        CameraSwitch.OnMapSwitch += SetMapActive;
        IsometricStarPosManager.OnIsometricStarView += OnIsometricStarView;
        RidePlanetSlingshot.OnSlingShot += OnSlingShot;
        RidePlanetSlingshot.OnExitPlanetOrbit += OnExitPlanetOrbit;

        transform.LookAt(firstStar);
    }


    private void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();

        collectDustSound = collectDustSoundContainer.GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider collision)
    {
        // TODO: Move me to the collect stardust script.
        if (collision.tag.Contains("|Dust|"))
        {
            collectDustSound.Play();
        }     
    }

    private void Update()
    {
        stardustCount.text = "Stardust: " + stardust;
    }

    private void FixedUpdate()
    {
        if (_mapActive) return;

        if (_isExitPlanetOrbit)
        {
            transform.position = _exitPlanetOrbitPos;
            _isExitPlanetOrbit = false;
        }

        // when on slingshot, make the player move towards the target 
        if (_onSlingShot)
        {
            var desiredPosition = _starToGo.position + disFromGoalStar;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, speed * 2 * Time.fixedDeltaTime);
    
            // TODO: disable all the figure when slingshot

            if (Vector3.Distance(desiredPosition, transform.position) < 200.0f)
            {
                if (_enableIsoViewMovement)
                {
                    _onSlingShot = false;
                    EndSlingshot();
                }
            }
            
            transform.LookAt(_starToGo);

        }
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            _movementInput = gamepad.leftStick.ReadValue();
        }

        _inputAction.Player.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _inputAction.Player.Move.canceled += ctx => _movementInput = Vector2.zero;

        var xAxisInput = _movementInput.x;
        var yAxisInput = -1 * _movementInput.y;

        // limit the degree of player rotation
        var currentRotation = transform.eulerAngles;
        
        // unity is behaving weird. 0 is -360
        var curRotationTmpX = currentRotation.x - 360;
        
        if (currentRotation.x >= 80 && currentRotation.x <=89)
        {
            currentRotation.x = 80;
            transform.eulerAngles = currentRotation;
        }
        else if (curRotationTmpX <= -80 && curRotationTmpX >= -90)
        {
            currentRotation.x = -80 + 360;
            transform.eulerAngles = currentRotation;
        }
  
        if (_enableIsoViewMovement)
        {
            // TODO: move the player relative to the plane and the star positions (2D)
            if (_firstTimeEnterIso)
            {
                transform.position = playerIsoEnterPos;
                _firstTimeEnterIso = false;
            }
            
            // move player in the direction of the star
            var playerPos = transform.position;

            var playerIconPos = playerIcon.position;
            var starPos = _curStar.position;
            var dir = (starPos - playerIconPos).normalized;
            
            // do not let player get too close to star
            if (Vector3.Distance(starPos, playerPos + dir * yAxisInput) <= 20)
            {
                var dirStar = (playerPos - starPos).normalized;
                transform.position = starPos + dirStar * 20;
            }
            else
            {
                transform.position = playerPos + dir * yAxisInput;
                            
            }
            transform.LookAt(_curStar);
            
        } else
        {
            if (_firstTimeExitIso)
            {
                transform.position = playerIsoExitPos;
                // make the player look at the edge of the gravitational field to have a sense of direction
                var starDirToPlayer = transform.position - _curStar.position;
                starDirToPlayer.x += 200.0f;
                var edge = _curStar.position + starDirToPlayer.normalized * 180;
                transform.LookAt(edge);
                _firstTimeExitIso = false;
            }
            
            
            if (Math.Abs(xAxisInput) > 0.1f || Math.Abs(yAxisInput) > 0.1f)
            {
                // Squaring the inputs makes finer movements easier.
                var interpretedXInput = xAxisInput * xAxisInput * maximumTurnRate;
                var interpretedYInput = yAxisInput * yAxisInput * maximumTurnRate;
            
                // But squaring negative values makes them positive.
                if (xAxisInput < 0)
                {
                    interpretedXInput = -interpretedXInput;
                }
                if (yAxisInput < 0)
                {
                    interpretedYInput = -interpretedYInput;
                }
    
                transform.Rotate(interpretedYInput, interpretedXInput, 0, Space.Self);
            }   
            var transform2 = transform;
            transform2.position += transform2.forward * speed;
        }
    }
    
    private void EndSlingshot()
    {
        _beginSlingshot = false;
        OnEndSlingShot?.Invoke(_beginSlingshot);
    }
    
    private void OnSlingShot(bool onSlingShot, Transform starToGo)
    {
        print("+++++++++++++  player is on slingshot");
        _onSlingShot = onSlingShot;
        _starToGo = starToGo;
    }

    private static void SetMapActive(bool mapActive)
    {
        _mapActive = mapActive;
    }
    
    private void OnIsometricStarView(bool onIso, Transform star)
    {
        Debug.Log("camera -- OnIsometricStarView");
        _enableIsoViewMovement = onIso;
        _firstTimeEnterIso = true;
        _firstTimeExitIso = true;
        _curStar = star;
        
        print("OnIsometricStarView -- " + onIso);
    }
    
    private void OnExitPlanetOrbit(Vector3 newPos)
    {
        _isExitPlanetOrbit = true;
        _exitPlanetOrbitPos = newPos;
    }
    

    //InputActions
    //Activates all actions in Player action maps (action maps are Player and UI)
    private void OnEnable()
    {
        _inputAction.Player.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        _inputAction.Player.Disable();
        CameraSwitch.OnMapSwitch -= SetMapActive;
        IsometricStarPosManager.OnIsometricStarView -= OnIsometricStarView;
        RidePlanetSlingshot.OnSlingShot -= OnSlingShot;
    }
    
}
