using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to an object that would be the epicentre of the orbit,
/// and forces provided |GravityObject| to orbit around it.
/// The speed of the orbit is proportional to the |GravityObject| mass.
/// </summary>
/// <remarks>
/// 
/// </remarks>
public class Orbit : MonoBehaviour
{

    private GameObject[] _orbiters;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
