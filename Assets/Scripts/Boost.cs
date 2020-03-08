using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Boost : MonoBehaviour
{
    private ThirdPersonPlayer _playerScript;
    private float _normalSpeed;
    private float _boostSpeed;

    //InputActions
    PlayerInputActions inputAction;
    
    public InputAction boostAction;

    void Awake()
    {
        //InputActions
        inputAction = new PlayerInputActions();
        boostAction = inputAction.Player.Boost;
    }

    private void Start()
    {
        _playerScript = GameObject.FindWithTag("|Player|").GetComponent<ThirdPersonPlayer>();
        _normalSpeed = _playerScript.speed;
        _boostSpeed = _normalSpeed * 3;
    }

    void Update()
    {
        if (_playerScript.stardust >= 3)
        {
            //InputAction replaces "Input.GetButton("Example") and holds a bool
            _playerScript.speed = boostAction.triggered ? _boostSpeed : _normalSpeed;
            
            //_playerScript.speed = Input.GetButton("Jump") ? _boostSpeed : _normalSpeed;
        }
    }

    //InputActions
    //Activates all actions in Player action maps (action maps are Player and UI)
    private void OnEnable()
    {
        boostAction.Enable();
        inputAction.Player.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        boostAction.Disable();
        inputAction.Player.Disable();
    }
}
