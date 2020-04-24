using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Boost : MonoBehaviour
{
    private ThirdPersonPlayer _playerScript;
    private float _normalSpeed;
    private float _boostSpeed;

    //InputActions
    PlayerInputActions _inputAction;
    
    public InputAction boostAction;

    private bool _mPressed;
    private bool _mReleased;
    
    public static event Action<bool> OnBoost;
    private bool _canBoost;
    private bool _remindBoost;
    
    
    void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();
        boostAction = _inputAction.Player.Boost;
        _remindBoost = true;
    }

    private void Start()
    {
        _playerScript = GetComponent<ThirdPersonPlayer>();
        _normalSpeed = _playerScript.speed;
        _boostSpeed = _normalSpeed * 3;
    }

    void Update()
    {
        if (_playerScript.stardust >= 3)
        {
            boostAction.performed += ctx =>
            {
                _mPressed = true;
                _mReleased = false;
            };
            boostAction.canceled += ctx =>
            {
                _mReleased = true;
                _mPressed = false;
            }; 
            //InputAction replaces "Input.GetButton("Example") and holds a bool
            // _playerScript.speed = boostAction.triggered ? _boostSpeed : _normalSpeed;
            if (_mPressed) _playerScript.speed = _boostSpeed;
            if (_mReleased) _playerScript.speed = _normalSpeed;

            if (!_remindBoost) return;
            _canBoost = true;
            OnBoost?.Invoke(_canBoost);
            _remindBoost = false;
            
            Invoke(nameof(StopTutorial), 5);

        }
        
        
        // remind player to boost after 1.5 minutes
        Invoke(nameof(RemindBoost), 90);
    }

    private void StopTutorial()
    {
        if (!_remindBoost) return;
        _canBoost = false;
        OnBoost?.Invoke(_canBoost);
        _remindBoost = false;
    }
    private void RemindBoost()
    {
        _remindBoost = true;
    }

    //InputActions
    //Activates all actions in Player action maps (action maps are Player and UI)
    private void OnEnable()
    {
        boostAction.Enable();
        _inputAction.Player.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        boostAction.Disable();
        _inputAction.Player.Disable();
    }
}
