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

    //InputActions
    private PlayerInputActions _inputAction;
    
    //Movement
    private Vector2 _movementInput;
    private bool _levelCleared;
    private bool _enableIsoViewMovement;

    private bool _onSlingShot;
    private Transform _starToGo;
    public Vector3 disFromGoalStar = new Vector3(50, 0,50);

    public Vector3 playerIsoStartPos;
    private bool _firstTimeIso;
    private Transform _curStar;

    private void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();

        collectDustSound = collectDustSoundContainer.GetComponent<AudioSource>();
    }

    private void Start()
    {
        CameraSwitch.OnMapSwitch += SetMapActive;
        IsometricStarPosManager.OnIsometricStarView += OnIsometricStarView;
        StarIconManager.OnSelectStar += OnSlingShot;
        CreateStar.OnStarCreation += ConsumeStardust;
        DestroyStar.OnStarDestruction += AcquireStardust;
    }

    private void Update()
    {
        //TODO: move me to where stardust gets added or removed.
        stardustCount.text = "Stardust: " + stardust;
    }
    
    private void FixedUpdate()
    {
        if (_levelCleared || _mapActive) return;

        // when on slingshot, make the player move towards the target 
        if (_onSlingShot)
        {
            var desiredPosition = _starToGo.position + disFromGoalStar;
            //TODO: There's no need for fixedDeltaTime when called in FixedUpdate()
            transform.position = Vector3.Lerp(transform.position, desiredPosition, speed * 2 * Time.fixedDeltaTime);

            // TODO: disable all the figure when slingshot

            if (Vector3.Distance(desiredPosition, transform.position) < 10.0f)
            {
                _onSlingShot = false;
            }
            
        }
        
        _inputAction.Player.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _inputAction.Player.Move.canceled += ctx => _movementInput = Vector2.zero;

        var xAxisInput = _movementInput.x;
        var yAxisInput = -1 * _movementInput.y;

        if (_enableIsoViewMovement)
        {
            // TODO: move the player relative to the plane and the star positions (2D)
            if (_firstTimeIso)
            {
                transform.position = playerIsoStartPos;
                _firstTimeIso = false;
            }
            
            // move player in the direction of the star
            var playerPos = transform.position;
            var starPos = _curStar.position;
            var dir = (starPos - playerPos).normalized;
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
        
    }
    
    public void OnTriggerEnter(Collider collision)
    {
        // TODO: Move me to the collect stardust script.
        if (collision.tag.Contains("|Dust|"))
        {
            collectDustSound.Play();
        }     
    }
    
    private void ConsumeStardust()
    {
        stardust--;
    }
    
    private void AcquireStardust()
    {
        stardust++;
    }

    private void OnSlingShot(Transform starToGo)
    {
        _onSlingShot = true;
        _starToGo = starToGo;
    }
    
    private static void SetMapActive(bool mapActive)
    {
        _mapActive = mapActive;
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
        if (_levelCleared) return;
        _enableIsoViewMovement = onIso;
        _firstTimeIso = true;
        _curStar = star;
    }
    
}
