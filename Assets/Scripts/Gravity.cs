using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    // Update is called once per frame

    private GameObject[] _gravityObjects;
    public float gravityStrength;
    public float gravityRadius;
    
    private void Start()
    {
        try
        {
            _gravityObjects = GameObject.FindGameObjectsWithTag("GravityObject");
        }
        catch (UnityException)
        {
            print("No such tag");
        }
    }

    private void Update()
    {
        foreach (var gravityObject in _gravityObjects)
        {
            gravityObject.GetComponent<Rigidbody>().AddExplosionForce(
                -gravityStrength, 
                transform.position, 
                gravityRadius,
                0.0f,
                ForceMode.Force
                );
        }
        
    }
}
