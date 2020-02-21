using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When a player collides with an asteroid belt, reverses the player's direction of travel, but maintains their speed.
/// </summary>
public class AsteroidBeltBounce : MonoBehaviour
{
    // TODO: play around with explosionStrength and find the best value 
    public float explosionStrength = 1.0f;
    void OnCollisionEnter(Collision c)
    {
        Debug.Log(c.rigidbody.velocity.normalized);
        Vector3 forceVec = c.rigidbody.velocity.normalized * explosionStrength;
        c.rigidbody.AddForce(forceVec, ForceMode.Impulse);
    }
}
