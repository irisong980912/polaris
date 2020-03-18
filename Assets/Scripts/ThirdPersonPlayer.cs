using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

using UnityEngine.Experimental;

public class ThirdPersonPlayer : MonoBehaviour
{
    public float speed;
    public int stardust;
    public TextMeshProUGUI stardustCount;
    public List<GameObject> inventory = new List<GameObject>();

    public Transform cam;

    private static bool _mapActive;

    public int stardustSelection;

    public AudioSource collectDustSound;
    public GameObject collectDustSoundContainer;

    //InputActions
    PlayerInputActions inputAction;
    
    //Movement
    Vector2 movementInput;
    private bool _levelCleared;
    private bool _enableIsometricViewMovement;


    private void Start()
    {
        CameraSwitch.OnMapSwitch += SetMapActive;
        IsometricStarView.OnEnterGravityField += OnEnterGravityField;
        IsometricStarView.OnExitGravityField += OnExitGravityField;
    }

    private static void SetMapActive(bool mapActive)
    {
        _mapActive = mapActive;
    }

    void Awake()
    {
        //InputActions
        inputAction = new PlayerInputActions();

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
    
        if (_levelCleared) return;
        
        /*
        var xAxisInput = Input.GetAxisRaw("Horizontal");
        var yAxisInput = Input.GetAxisRaw("Vertical");
         */

        //InputAction replaces "Input.GetAxis("Example")" and calls function
        //movementInput = inputAction.Player.Move.ReadValue<Vector2>();
        
        if (_mapActive) return;
        
        inputAction.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputAction.Player.Move.canceled += ctx => movementInput = Vector2.zero;


        var xAxisInput = movementInput.x;
        var yAxisInput = movementInput.y;
        
        // TODO: move the player in a 2D perspective
        if (_enableIsometricViewMovement)
        {
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
        
        
        
    }

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
    
    private void OnLevelClear()
    {
        _levelCleared = true;
    }

    private void OnEnterGravityField()
    {
        Debug.Log("camera -- OnEnterGravityField");
        if (_levelCleared) return;
        _enableIsometricViewMovement = true;

    }
    
    private void OnExitGravityField()
    {
        Debug.Log("camera -- OnExitGravityField");
        if (_levelCleared) return;
        _enableIsometricViewMovement = false;

    }


}
