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
    // public static event Action OnOrbitStart; 
    // public static event Action OnOrbitStop;
    // public static event Action<bool, Transform> OnSlingShot;
    
    //InputActions
    private PlayerInputActions _inputAction;

    private bool _isLit;
    
    private Transform _starToGo;
    public Transform coreForPlayer;
    

    void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();
    }

    private void Start()
    {
        _planetSpeed = 0;
        _playerSpeed = playerDarkStarSpeed;

        CreateStar.OnStarCreation += OnStarCreation;
        DestroyStar.OnStarDestruction += OnStarDestruction;
        _self = transform;

    }
    
    // private void Update()
    // {
    //     _isLit = transform.parent.GetComponent<Star>().isCreated;
    // }


    private void FixedUpdate()
    {
        // TODO: figure out player orbit angle or change the planet positions

        if (gameObject.tag.Contains("|GravityCore|"))
        {
            // check if the parent star is lit  to prevent all the planets in the scene from orbiting
            if (transform.parent.GetComponent<Star>().isCreated)
            {
                // star self-rotate
                _self.Rotate(_self.up * _planetSpeed, Space.World);
            }
            coreForPlayer.Rotate(_self.up * _playerSpeed, Space.World);
        }
        // else if (gameObject.tag.Contains("|PlanetCore|"))
        // {
        //     // planet self-rotate
        //     _self.Rotate(_self.up * _planetSpeed, Space.World);
        // }
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
        // else if (gameObject.tag.Contains("|PlanetCore|"))
        // {
        //     if (!other.gameObject.tag.Contains("|Player|")) return;
        //     
        //     print("collide with a planet! " + gameObject.transform.parent.name);
        //     
        //     // player can orbit the planet only if the star is lit
        //     if (Math.Abs(_planetSpeed) < 0.01) return;
        //     _player = other;
        //     _self.LookAt(other.transform.position);
        //     other.gameObject.transform.SetParent(transform);
        //     
        //     OnOrbitStart?.Invoke();
        // }
        
    }

    /// <summary>
    /// When objects leave the gravitational field, they cease to be children of _self, so they stop rotating when _self spins.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        other.gameObject.transform.SetParent(null);
        _player = null;
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
