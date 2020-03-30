using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ThirdPersonPlayer : MonoBehaviour

{
    public Transform cam;
    public float speed;
    public float maximumTurnRate;

    public int stardust;
    public TextMeshProUGUI stardustCount;
    public List<GameObject> inventory = new List<GameObject>();

    private static bool _mapActive;

    public AudioSource collectDustSound;
    public GameObject collectDustSoundContainer;

    // public Transform firstStar;

    //InputActions
    private PlayerInputActions _inputAction;
    
    //Movement
    private Vector2 _movementInput;
    private bool _levelCleared;
    private bool _enableIsoViewMovement;

    private bool _onSlingShot;
    private Transform _starToGo;
    public Vector3 disFromGoalStar = new Vector3(50, 0,50);

    private bool _beginSlingshot;
    public static event Action<bool> EndSlingShot;

    public Vector3 playerIsoStartPos;
    private bool _firstTimeIso;
    private Transform curStar;


    private void Start()
    {
        CameraSwitch.OnMapSwitch += SetMapActive;
        IsometricStarPosManager.OnIsometricStarView += OnIsometricStarView;
        Orbit.OnSlingShot += OnSlingShot;
        // transform.LookAt(firstStar);
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
    
    private void endSlingshot()
    {
        _beginSlingshot = false;
        EndSlingShot?.Invoke(_beginSlingshot);
    }

    private void FixedUpdate()
    {
        if (_levelCleared) return;

        if (_mapActive) return;

        // when on slingshot, make the player move towards the target 
        if (_onSlingShot)
        {
            var desiredPosition = _starToGo.position + disFromGoalStar;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, speed * 2 * Time.fixedDeltaTime);

            // TODO: disable all the figure when slingshot

            if (Vector3.Distance(desiredPosition, transform.position) < 10.0f)
            {
                _onSlingShot = false;
                endSlingshot();
            }
            
        }
        
        _inputAction.Player.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _inputAction.Player.Move.canceled += ctx => _movementInput = Vector2.zero;

        var xAxisInput = _movementInput.x;
        var yAxisInput = -1 * _movementInput.y;

        if (_enableIsoViewMovement)
        {
            // TODO: move the player in a 2D perspective
            if (_firstTimeIso)
            {
                transform.position = playerIsoStartPos;
                _firstTimeIso = false;
            }
            
            // move player in the direction of the star
            var playerPos = transform.position;
            var starPos = curStar.position;

            var dir = (starPos - playerPos).normalized;
            
            // playerPos.x += xAxisInput;
            // playerPos.z += yAxisInput;
            transform.position = playerPos + dir * xAxisInput;
            
            transform.LookAt(cam.transform);
            

        }
        else
        {
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

                Transform transform1;
                (transform1 = transform).Rotate(interpretedYInput, interpretedXInput, 0, Space.Self);
                var q = transform1.rotation;
                q.eulerAngles = new Vector3(q.eulerAngles.x, q.eulerAngles.y, 0);
                transform1.rotation = q;
            }   
            var transform2 = transform;
            transform2.position += transform2.forward * speed;
        }

        // var transform2 = transform;
        // transform2.position += transform2.forward * speed;
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
    }
    
    private void OnIsometricStarView(bool onIso, Transform star)
    {
        Debug.Log("camera -- OnIsometricStarView");
        if (_levelCleared) return;
        _enableIsoViewMovement = onIso;
        _firstTimeIso = true;
        curStar = star;

    }
}
