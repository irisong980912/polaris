using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Serialization;

public class ThirdPersonPlayer : MonoBehaviour

{
    
    public Transform cam;
    public float speed;
    public float rotateSpeed = 5.0f;

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
        
        _inputAction.Player.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _inputAction.Player.Move.canceled += ctx => _movementInput = Vector2.zero;

        if (_mapActive) return;
        
        // var xAxisInput = _movementInput.x;
        // var yAxisInput = _movementInput.y;
        
        // make player travel to the direction that camera is facing
        transform.position = transform.position + cam.transform.forward * speed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, cam.rotation, rotateSpeed);
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
