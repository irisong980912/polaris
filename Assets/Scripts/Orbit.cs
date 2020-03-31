using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// apply  this script to star gravityCore
/// 
/// Attached to an object that would be the epicentre of the orbit,
/// and forces provided |GravityObject| to orbit around it.
/// The speed of the orbit is determined by the speed variable.
/// </summary>
/// <remarks>
/// Orbits are achieved by attaching orbiting objects as children to transform, and then rotating transform.
/// </remarks>
/// <param>
/// speed: determines how quickly the attached objects rotate.
/// cam: the main camera of the scene.
/// </param>
public class Orbit : MonoBehaviour
{
    public float planetLitStarSpeed;
    private float _planetSpeed;

    public float playerDarkStarSpeed;
    public float playerLitStarSpeed;
    private float _playerSpeed;

    private Collider _player;
    private Transform _self;
    public static event Action OnOrbitStart;
    public static event Action OnOrbitStop;
    public static event Action<Transform> OnSlingShot;

    //InputActions
    private PlayerInputActions _inputAction;

    public InputAction slingshotAction;
    public Transform coreForPlayer;

    private void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();
        slingshotAction = _inputAction.Player.Slingshot;
    }

    private void Start()
    {
        _planetSpeed = 0;
        _playerSpeed = playerDarkStarSpeed;
        
        StarIconManager.OnSelectStar += Slingshot;
        CreateStar.OnStarCreation += OnStarCreation;
        DestroyStar.OnStarDestruction += OnStarDestruction;
        _self = transform;
    }
    
    private void FixedUpdate()
    {
        // TODO: figure out player orbit angle or change the planet positions
        _self.Rotate(_self.up * _planetSpeed, Space.World);

        if (gameObject.tag.Contains("|GravityCore|"))
        {
            coreForPlayer.Rotate(_self.up * _playerSpeed, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag.Contains("|GravityCore|"))
        {
            if (other.gameObject.tag.Contains("|Planet|"))
            {
                other.gameObject.transform.SetParent(transform);
            }
            // TODO: make player orbit in the same direction as the planet but at a faster speed
            else if (other.gameObject.tag.Contains("|Player|"))
            {
                other.gameObject.transform.SetParent(coreForPlayer);
            }
            
        }
        else if (gameObject.tag.Contains("|PlanetCore|"))
        {
            if (!other.gameObject.tag.Contains("|Player|")) return;
            // player can orbit planets only if the star is lit
            // _planetSpeed == 0 when the star is not lit.
            if (Math.Abs(_planetSpeed) < 0.01) return;

            _player = other;
            _self.LookAt(other.transform);
            other.gameObject.transform.SetParent(transform);
            
            OnOrbitStart?.Invoke();
        }
        
    }

    /// <summary>
    /// When objects leave their orbits, they cease to be children of _self, so they stop rotating when _self spins.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        other.gameObject.transform.SetParent(null);
        _player = null;
        
        if (!other.gameObject.tag.Contains("|Player|")) return;
        OnOrbitStop?.Invoke();
    }

    private void OnStarDestruction()
    {
        _planetSpeed = 0;
        _playerSpeed = playerDarkStarSpeed;
    }

    private void OnStarCreation()
    {
        _planetSpeed = planetLitStarSpeed;
        _playerSpeed = playerLitStarSpeed;
    }

    private void Slingshot(Transform starToGo)
    {
        OnSlingShot?.Invoke(starToGo);
        _player.gameObject.transform.SetParent(null);
        _player = null;
    }

    //InputActions
    //Activates all actions in Player action maps (action maps are Player and UI)
    private void OnEnable()
    {
        slingshotAction.Enable();
        _inputAction.Player.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        slingshotAction.Disable();
        _inputAction.Player.Disable();
    }

}
