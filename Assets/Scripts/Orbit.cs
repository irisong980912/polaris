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
    
    private void FixedUpdate()
    {
        transform.Rotate(transform.up, speed);
    }

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
            other.transform.forward = other.transform.position - transform.position;
            other.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.transform.SetParent(null);
    }
}
