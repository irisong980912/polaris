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

    public Camera cam;
    private Collider _player;
    private Transform _self;
    
    private bool _launchBegan;
    public static event Action OnOrbitStart; 
    public static event Action OnOrbitStop;
    public static event Action<bool, Transform> OnSlingShot;
    
    private bool _onSlingShot;
    private bool _beginSlingShot;

    //InputActions
    private PlayerInputActions _inputAction;
    public InputAction slingshotAction;
    
    private bool _isLit;
    
    private Transform _starToGo;
    public Transform coreForPlayer;
    

    void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();
        slingshotAction = _inputAction.Player.Slingshot;
    }

    private void Start()
    {
        _planetSpeed = 0;
        _playerSpeed = playerDarkStarSpeed;
        
        StarIconManager.OnSelectStar += OnSelectStar;
        // listen for player movement to stop the slingshot when arriving at designated location
        ThirdPersonPlayer.EndSlingShot += EndSlingShot;
        
        CreateStar.OnStarCreation += OnStarCreation;
        DestroyStar.OnStarDestruction += OnStarDestruction;
        _self = transform;

    }

    private void FixedUpdate()
    {
        // TODO: figure out player orbit angle or change the planet positions
        
        // check if the parent star is lit  to prevent all the planets in the scene from orbiting
        if (transform.parent.GetComponent<Star>().isCreated)
        {
            _self.Rotate(_self.up * _planetSpeed, Space.World);
        }

        if (gameObject.tag.Contains("|GravityCore|"))
        {
            coreForPlayer.Rotate(_self.up * _playerSpeed, Space.World);
        }
        
        
        if (_player is null) return;
        if (!_isLit) return;
        if (!_starToGo  || _player.transform.parent != _self) return;
        if (!_beginSlingShot) return;
        if (_launchBegan) return;
        print("orbit --- launch begin" + _launchBegan);
        SlingshotStart();
    }

    //TODO: mark these comments as a TODO item, add them to documentation, or delete them.
    // need to check if the star is lit or not
    // check if create star or destroy star is on trigger
    /// <summary>
    /// When an object capable of orbiting enters the collider, it becomes a child of _self.
    /// </summary>
    /// <remarks>
    /// Since players can move, we need to regularly call AdjustRotation() to keep a player orbiting
    /// all the way around the object.
    /// </remarks>
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
                print("orbit ----- set player to core");
                other.gameObject.transform.SetParent(coreForPlayer);
            }
        }
        else if (gameObject.tag.Contains("|PlanetCore|"))
        {
            if (!other.gameObject.tag.Contains("|Player|")) return;
            
            print("collide with a planet! " + gameObject.transform.parent.name);
            
            // player can orbit the planet only if the star is lit
            if (Math.Abs(_planetSpeed) < 0.01) return;
            _player = other;
            _self.LookAt(other.transform.position);
            other.gameObject.transform.SetParent(transform);
            
            OnOrbitStart?.Invoke();
            // TODO: remove adjust rotation
            InvokeRepeating(nameof(AdjustRotation), 1.0f, 1.0f);
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
        print("slingshot - trigger onOrbitStop");
        OnOrbitStop?.Invoke();
        CancelInvoke(nameof(AdjustRotation));
    }
    
    /// <summary>
    /// Sometimes the player will not orbit cleanly around the planet, instead circling a "halo" path around it.
    /// This method will adjust the spinning planet core to realign the player's orbit around the planet.
    /// </summary>
    private void AdjustRotation()
    {
        _player.gameObject.transform.SetParent(null);
        _self.forward = _player.transform.position - _self.position;
        _player.gameObject.transform.SetParent(_self);
    }
    
    private void OnStarDestruction()
    {
        _isLit = false;
        _planetSpeed = 0;
        _playerSpeed = playerDarkStarSpeed;
    }

    private void OnStarCreation()
    {
        _isLit = true;
        _planetSpeed = planetLitStarSpeed;
        _playerSpeed = playerLitStarSpeed;
    }

    /// <summary>
    /// Begins the process of launching the player out of orbit.
    /// </summary>
    /// <remarks>
    /// The player will complete at least one orbit before launching, and speed up at a constant rate
    /// throughout this final rotation.
    /// </remarks>
    private void SlingshotStart()
    {
        print("slingshot start");
        _launchBegan = true;
        CancelInvoke(nameof(AdjustRotation));
        _player.gameObject.transform.SetParent(null);
        _self.forward = cam.transform.forward;
        _player.gameObject.transform.SetParent(_self);
        _planetSpeed = 3.0f;
        Invoke(nameof(Slingshot), 1.5f);
    }
    
    /// <summary>
    /// Launches the player after 1 rotation.
    /// </summary>
    private void Slingshot()
    {
        print("|||||||||| orbit --- slngshot ||||||||||");
        _onSlingShot = true;
        OnSlingShot?.Invoke(_onSlingShot, _starToGo);
        _launchBegan = false;
        _player.gameObject.transform.SetParent(null);
        _onSlingShot = false;
    }
    
    private void OnSelectStar(Transform starToGo)
    {
        print("|||||||||| orbit --- selected star ||||||||||");
        _starToGo = starToGo;
        _beginSlingShot = true;
    }
    
    private void EndSlingShot(bool obj)
    {
        _beginSlingShot = false;
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
