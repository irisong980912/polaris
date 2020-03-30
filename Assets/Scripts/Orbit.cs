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
    
    // public float speed;
    // public float playerOrbitSpeed;
    
    public Camera cam;
    private float _normalSpeed;
    private Collider _player;
    private Transform _self;
    private bool _launchBegan;
    public static event Action OnOrbitStart;
    public static event Action OnOrbitStop;
    public static event Action<bool, Transform> OnSlingShot;

    //InputActions
    PlayerInputActions inputAction;

    public InputAction slingshotAction;
    private Transform _starToGo;
    private bool _onSlingShot;
    private bool _beginSlingShot;
    public Transform coreForPlayer;
    private bool _isLit;

    void Awake()
    {
        //InputActions
        inputAction = new PlayerInputActions();
        slingshotAction = inputAction.Player.Slingshot;
    }

    private void Start()
    {
        _planetSpeed = 0;
        _playerSpeed = playerDarkStarSpeed;
        
        StarIconManager.OnSelectStar += OnSelectStar;
        ThirdPersonPlayer.EndSlingShot += EndSlingShot;
        CreateStar.OnStarCreation += OnStarCreation;
        DestroyStar.OnStarDestruction += OnStarDestruction;
        _self = transform;
        // Store the normal rotation speed so it can be restored after a slingshot.
        _normalSpeed = _planetSpeed;
        
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

    private void FixedUpdate()
    {
        _self.Rotate(_self.up * _planetSpeed, Space.World);

        if (gameObject.tag.Contains("|GravityCore|"))
        {
            coreForPlayer.Rotate(_self.up * _playerSpeed, Space.World);
        }
        
        
        if (_player is null) return;

        if (!_isLit) return;
        //if (!Input.GetButton("Fire2") || _player.transform.parent != _self) return;
        
        // haven't selected a star
        if (!_starToGo  || _player.transform.parent != _self) return;
        ////InputAction replaces "Input.GetButton("Example") and holds a bool
        // if (!slingshotAction.triggered || _player.transform.parent != _self) return;

        if (!_beginSlingShot) return;

        if (_launchBegan) return;
        //InputAction replaces "Input.GetButton("Example") and calls function
        //inputAction.Player.Interact.performed += ctx => SlingshotStart();
        print("orbit --- launch begin" + _launchBegan);
        SlingshotStart();
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
            
            // if (other.gameObject.tag.Contains("|Planet|"))
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
            
            print("clide with a plabet! " + gameObject.transform.parent.name);
            // player can orbit only if the star is lit
            if (!_self.parent.parent.GetComponent<Star>().isCreated) return;

            _player = other;
            _self.LookAt(other.transform.position);
            other.gameObject.transform.SetParent(transform);
            
            OnOrbitStart?.Invoke();
            InvokeRepeating(nameof(AdjustRotation), 1.0f, 1.0f);
        }
        
    }

    /// <summary>
    /// When objects leave their orbits, they cease to be children of _self, so they stop rotating when _self spins.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        other.gameObject.transform.SetParent(null);
        
        if (!other.gameObject.tag.Contains("|Player|")) return;


        print("slingshot - trigger onOxrbitStop");
        OnOrbitStop?.Invoke();

        CancelInvoke(nameof(AdjustRotation));
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

        // speed = _normalSpeed;
        // _player.gameObject.GetComponent<Rigidbody>().AddForce(_player.transform.forward.normalized * 50000, ForceMode.Force);
        //
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
        inputAction.Player.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        slingshotAction.Disable();
        inputAction.Player.Disable();
    }

}
