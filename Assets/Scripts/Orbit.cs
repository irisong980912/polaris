 using System.Collections.Generic;
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
    
    /// <summary>
    /// Rotates transform.
    /// </summary>
    void FixedUpdate()
    {
        transform.Rotate(transform.up, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.transform.SetParent(null);
    }
}
