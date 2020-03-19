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
    public float speed;
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

    void Awake()
    {
        //InputActions
        inputAction = new PlayerInputActions();
        slingshotAction = inputAction.Player.Slingshot;
    }

    private void Start()
    {
        StarIconManager.OnSelectStar += OnSelectStar;
        _self = gameObject.tag.Contains("|GravityCore|") ? transform.parent : transform;

        // Store the normal rotation speed so it can be restored after a slingshot.
        _normalSpeed = speed;
        
    }
    
    private void FixedUpdate()
    {
        _self.Rotate(_self.up, speed);
        
        if (_player is null) return;

        //if (!Input.GetButton("Fire2") || _player.transform.parent != _self) return;
        
        // haven't selected a star
        if (!_starToGo  || _player.transform.parent != _self) return;
        ////InputAction replaces "Input.GetButton("Example") and holds a bool
        // if (!slingshotAction.triggered || _player.transform.parent != _self) return;

        if (!_launchBegan)
        {
            //InputAction replaces "Input.GetButton("Example") and calls function
            //inputAction.Player.Interact.performed += ctx => SlingshotStart();

            SlingshotStart();
        }
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
            if (other.gameObject.tag.Contains("|Planet|"))
            {
                other.gameObject.transform.SetParent(transform.parent.transform);
            }
            
        }
        else if (gameObject.tag.Contains("|PlanetCore|"))
        {
            if (!other.gameObject.tag.Contains("|Player|")) return;
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
        speed = 3.0f;
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
        

        // speed = _normalSpeed;
        // _player.gameObject.GetComponent<Rigidbody>().AddForce(_player.transform.forward.normalized * 50000, ForceMode.Force);
        //
    }
    
    private void OnSelectStar(Transform starToGo)
    {
        print("|||||||||| orbit --- selected star ||||||||||");
        _starToGo = starToGo;
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
