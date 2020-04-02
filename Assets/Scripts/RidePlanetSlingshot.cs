using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Attach to planet core
/// </summary>
public class RidePlanetSlingshot : MonoBehaviour
{
    private Collider _player;

    private bool _launchBegan;
    
    // event observers
    public static event Action<bool> OnRidePlanet;
    private bool _isOnPlanet;
    
    public static event Action<bool, Transform> OnSlingShot;
    private bool _onSlingShot;
    
    // inform whether the slingshot ends
    private bool _beginSlingShot;

    //InputActions
    private PlayerInputActions _inputAction;
    public InputAction slingshotAction;
    
    private bool _isLit;
    
    private Transform _starToGo;

    private Transform _playerOriginalParent;
    private Vector3 _playerOriginalPos;
    private Transform _self;

    public float rotateSpeed;


    void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();
        slingshotAction = _inputAction.Player.Slingshot;
    }

    private void Start()
    {
        _self = transform;
        StarSelectButtonManager.OnSelectStar += OnSelectStar;
        // listen for player movement to stop the slingshot when arriving at designated location
        ThirdPersonPlayer.EndSlingShot += EndSlingShot;
    }

    private void Update()
    {
        if (!transform.parent.parent.parent.CompareTag("|Star|")) return;
        _isLit = transform.parent.parent.parent.GetComponent<Star>().isCreated;
    }

    private void FixedUpdate()
    {
        // TODO: figure out player orbit angle or change the planet positions
        
        // self rotate
        _self.Rotate(_self.up * rotateSpeed, Space.World);

        if (!gameObject.tag.Contains("|PlanetCore|")) return;
        if (_player is null) return;
        if (!_isLit) return;
        if (!_starToGo) return;
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
        if (!other.gameObject.tag.Contains("|Player|")) return;
        print("collide with a planet! " + gameObject.transform.parent.name);
        
        _player = other;
        _playerOriginalParent = _player.transform.parent.transform;
        print("player original parent -- " + _playerOriginalParent.name);
        
        // player can orbit the planet only if the star is lit
        if (!_isLit) return;
        
        print("!!!!!!!!!!! star is lit and ready to reveal the GUI");

        _self.LookAt(other.transform.position);
        other.gameObject.transform.SetParent(transform);
        
        // riding planet
        _isOnPlanet = true;
        OnRidePlanet?.Invoke(_isOnPlanet);
        
        // TODO: remove adjust rotation
        InvokeRepeating(nameof(AdjustRotation), 1.0f, 1.0f);
    
        
    }

    /// <summary>
    /// When objects leave the planet orbit, they cease to be children of _self, so they stop rotating when _self spins.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.tag.Contains("|Player|")) return;
        print("EXIT planet collide! " + gameObject.transform.parent.name);
        other.transform.SetParent(_playerOriginalParent);
        // when exit, reposition player
        RepositionPlayer(other.transform.position);
        _player = null;
        
        if (!other.gameObject.tag.Contains("|Player|")) return;
        print("slingshot - trigger onOrbitStop");
        //
        _isOnPlanet = false;
        OnRidePlanet?.Invoke(_isOnPlanet);
        CancelInvoke(nameof(AdjustRotation));
    }

    private void RepositionPlayer(Vector3 playerPos)
    {
        var playerStarDis = Vector3.Distance(playerPos, transform.parent.parent.position);
        var planetStarDis = Vector3.Distance(transform.position, transform.parent.parent.position);

        var dir = transform.parent.parent.position - transform.position;
        
        if (playerStarDis < planetStarDis)
        {
            print("player is closer to star than riding planet");
            var newPos = transform.position + dir * 5;
        }
        else
        {
            print("player is further from star than riding planet");
            var newPos = transform.position - dir * 5;
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
        _player.gameObject.transform.SetParent(null); ;
        _player.gameObject.transform.SetParent(_self);

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
        _onSlingShot = false;
    }
    
    private void OnSelectStar(Transform starToGo)
    {
        print("|||||||||| orbit --- selected star ||||||||||");
        _starToGo = starToGo;
        _beginSlingShot = true;
    }
    
    /// <summary>
    /// terminate the slingshot action when notified by the ThirdPersonPlayer script that
    /// the player has arrived at the desinated location 
    /// </summary>
    /// <param name="obj"></param>
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
        StarSelectButtonManager.OnSelectStar -= OnSelectStar;
        // listen for player movement to stop the slingshot when arriving at designated location
        ThirdPersonPlayer.EndSlingShot -= EndSlingShot;

    }

}