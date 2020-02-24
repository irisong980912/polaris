using UnityEngine;

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

    private void Start()
    {
        print("Orbit start "  + transform.name);
        if (gameObject.tag.Contains("|GravityCore|")) // the star gravity core
        {
            _self = transform.parent.transform; 
        }
        else  // the planet core
        {
            _self = transform;

        }
        
        _normalSpeed = speed;
    }
    
    private void FixedUpdate()
    {
        _self.Rotate(_self.up, speed);
        
        if (_player is null) return;
        
        if (!Input.GetButton("Fire1") || _player.transform.parent != _self) return;

        ////// !!!!!!!
        if (!_self.parent.parent.CompareTag("|Star|")) return;
        if (!_self.parent.parent.GetComponent<Star>().isCreated) return;

        if (!_launchBegan)
        {
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
    /// all the way around the player
    /// </remarks>
    /// <param name="other"> An object's collider that collides with the collider of _self. </param>
    private void OnTriggerEnter(Collider other)
    {

        if (gameObject.tag.Contains("|GravityCore|"))
        {
            if (other.gameObject.tag.Contains("|Planet|"))
            {

                print("Orbit -- add planet");

                other.gameObject.transform.SetParent(transform.parent.transform);
            }
        }
        else if (gameObject.tag.Contains("|PlanetCore|"))
        {
            if (!other.gameObject.tag.Contains("|Player|")) return;
            cam.GetComponent<ThirdPersonCamera>().OrbitDetected(_self);
            _player = other;
            _self.forward = other.transform.position - transform.position;
            other.gameObject.transform.SetParent(transform);
            InvokeRepeating(nameof(AdjustRotation), 1.0f, 1.0f);
        }
    }

    /// <summary>
    /// When objects leave their orbits, they cease to be children of _self, so they stop rotating when _self spins.
    /// </summary>
    /// <param name="other"> An object's collider that collides with the collider of _self. </param>
    private void OnTriggerExit(Collider other)
    {
        other.gameObject.transform.SetParent(null);
        
        if (!other.gameObject.tag.Contains("|Player|")) return;
        cam.GetComponent<ThirdPersonCamera>().CancelFocus();
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
        _launchBegan = true;
        CancelInvoke(nameof(AdjustRotation));
        _player.gameObject.transform.SetParent(null);
        _self.forward = cam.transform.forward;
        _player.gameObject.transform.SetParent(_self);
        speed = 7.2f;
        Invoke(nameof(Slingshot), 1.0f);
    }
    
    /// <summary>
    /// Launches the player after 1 rotation.
    /// </summary>
    private void Slingshot()
    {
        _launchBegan = false;
        _player.gameObject.transform.SetParent(null);
        speed = _normalSpeed;
        _player.gameObject.GetComponent<Rigidbody>().AddForce(cam.transform.forward.normalized * 6000, ForceMode.Force);
    }
}
