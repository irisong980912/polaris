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

    //InputActions
    private PlayerInputActions _inputAction;
    
    //Movement
    private Vector2 _movementInput;


    private void Start()
    {
        CameraSwitch.OnMapSwitch += SetMapActive;
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

        Debug.Log(stardust);
    }

    private void FixedUpdate()
    {
        //InputAction replaces "Input.GetAxis("Example")" and calls function
        //movementInput = inputAction.Player.Move.ReadValue<Vector2>();
        _inputAction.Player.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _inputAction.Player.Move.canceled += ctx => _movementInput = Vector2.zero;

        if (_mapActive) return;
        
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
            
            // An upwards rotation (from the Mouse Y Axis) is a rotation about the X Axis.
            // Similarly, a sideways rotation (from Mouse X) is a rotation about the Y Axis.
            transform.Rotate(interpretedYInput, interpretedXInput, 0, Space.Self);
        }
        
        transform.Translate(transform.forward * speed);
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

}
