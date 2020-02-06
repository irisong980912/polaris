using UnityEngine;

/// <summary>
/// Attached to an object that would be the epicentre of the orbit,
/// and forces provided |GravityObject| to orbit around it.
/// The speed of the orbit is determined by the speed variable.
/// </summary>
/// <param>
/// speed: determines how quickly the attached objects rotate.
/// </param>
public class Orbit : MonoBehaviour
{
    public float speed;
    public Camera cam;
    private Collider _player;
    private Transform _self;

    private void Start()
    {
        _self = transform;
    }

    private void FixedUpdate()
    {
        _self.Rotate(_self.up, speed);




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

    // need to check if the star is lit or not
    // check if create star or destroy star is on trigger
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

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.transform.SetParent(null);
        if (other.gameObject.tag.Contains("|Player|"))
        {
            cam.GetComponent<ThirdPersonCamera>().CancelFocus();
            CancelInvoke(nameof(AdjustRotation));
        }
    }
}
