using UnityEngine;

/// <summary>
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
    private bool _launchReady;

    private void Start()
    {
        _self = transform;
        _normalSpeed = speed;
    }
    
    private void FixedUpdate()
    {
        _self.Rotate(_self.up, speed);

        if (!Input.GetButton("Fire1") || _player.transform.parent != _self) return;
        if (!_launchBegan)
        {
            SlingshotStart();
        }
        else
        {
            Slingshot();
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
        if (gameObject.tag.Contains("|Star|"))
        {
            if (other.gameObject.tag.Contains("|Planet|"))
            {
                other.gameObject.transform.SetParent(transform);
            }
        }
        else if (gameObject.tag.Contains("|PlanetCore|"))
        {
            if (!other.gameObject.tag.Contains("|Player|")) return;
            cam.GetComponent<ThirdPersonCamera>().OrbitDetected(_self);
            _player = other;
            _self.forward = other.transform.position - transform.position;
            other.gameObject.transform.SetParent(_self);
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
        _self.forward = cam.transform.forward;
        InvokeRepeating(nameof(SpeedUp), 0, 0.02f); // 0.02 seconds is consistent with FixedUpdate.
    }
    
    /// <summary>
    /// Launches the player after they have gained enough speed.
    /// </summary>
    private void Slingshot()
    {
        if (!(Vector3.Angle(cam.transform.forward, _self.forward) < 5)) return;
        if (!_launchReady) return;
        _launchReady = false;
        _launchBegan = false;
        _player.gameObject.transform.SetParent(null);
        speed = _normalSpeed;
    }

    /// <summary>
    /// Increases the speed of the object's rotation until it close to 10.
    /// </summary>
    /// <remarks>
    /// The original rotation speed is kept in _normalSpeed, which restores can be used to restore the rotation speed
    /// back to its original speed.
    /// </remarks>
    private void SpeedUp()
    {
        if (speed <= 9.75f)
        {
            speed += 0.25f;
        }
        else
        {
            _launchReady = true;
            CancelInvoke(nameof(SpeedUp));
        }
    }
    
}
