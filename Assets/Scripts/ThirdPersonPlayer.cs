using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ThirdPersonPlayer : MonoBehaviour

{
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
    private bool _enableIsometricViewMovement;

    private bool _onSlingShot;
    private Transform _starToGo;
    public Vector3 disFromGoalStar = new Vector3(50, 0,50);
    
    private bool _onIso;
    private bool _beginSlingshot;
    public static event Action<bool> EndSlingShot;


    private void Start()
    {
        CameraSwitch.OnMapSwitch += SetMapActive;
        IsometricStarView.OnIsometricStarView += OnIsometricStarView;
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
        
        /*
        var xAxisInput = Input.GetAxisRaw("Horizontal");
        var yAxisInput = Input.GetAxisRaw("Vertical");
         */

        //InputAction replaces "Input.GetAxis("Example")" and calls function
        //movementInput = inputAction.Player.Move.ReadValue<Vector2>();
        _inputAction.Player.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _inputAction.Player.Move.canceled += ctx => _movementInput = Vector2.zero;
        
        if (_mapActive) return;

        // when on slingshot, make the player move towards the target 
        if (_onSlingShot)
        {
            print("|||||||||| third person player --- slngshot ||||||||||" + _starToGo.name);
            
            // sling shot the player to the designated star starToGo
            var desiredPosition = _starToGo.position + disFromGoalStar;
            //transform.position = Vector3.MoveTowards(transform.position, desiredPosition, speed * 100 * Time.fixedDeltaTime);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, speed * 2 * Time.fixedDeltaTime);

            // TODO: disable all the figure when slingshot

            if (Vector3.Distance(desiredPosition, transform.position) < 10.0f)
            {
                print("DISABLE |||||||||| third person player --- slngshot ||||||||||");
                _onSlingShot = false;
                endSlingshot();

            }
        }
        
        inputAction.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputAction.Player.Move.canceled += ctx => movementInput = Vector2.zero;


        var xAxisInput = movementInput.x;
        var yAxisInput = movementInput.y;
        
        
        if (_enableIsometricViewMovement)
        {
            // TODO: move the player in a 2D perspective
            var playerPos = transform.position;
            playerPos.x += xAxisInput;
            playerPos.z += xAxisInput;
            transform.position = playerPos;
        }
        else
        {
            if (Math.Abs(xAxisInput) < 0.1f && Math.Abs(yAxisInput) < 0.1f) return;

            //var directionFromInput = new Vector3(xAxisInput, 0f, yAxisInput).normalized;
            var directionFromInput = new Vector3(0f, 0f, yAxisInput).normalized;

            var directionOfTravel = cam.TransformDirection(directionFromInput);
        
            transform.Translate(directionOfTravel * speed, Space.World);
            transform.forward = directionOfTravel;

        }
        
        var xAxisInput = _movementInput.x;
        var yAxisInput = _movementInput.y;
        
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
    
    private void OnIsometricStarView(bool onIso)
    {
        Debug.Log("camera -- OnIsometricStarView");
        if (_levelCleared) return;
        _onIso = onIso;
        _enableIsometricViewMovement = _onIso;
    }
}
