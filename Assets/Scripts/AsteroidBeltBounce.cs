using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When a player collides with an asteroid belt, reverses the player's direction of travel, but maintains their speed.
/// </summary>
public class AsteroidBeltBounce : MonoBehaviour
{
    // TODO: play around with explosionStrength and find the best value 
    public float explosionStrength = 1000.0f;
    void OnCollisionEnter(Collision c)
    {
        Vector3 forceVec = c.rigidbody.velocity.normalized * explosionStrength;
        c.rigidbody.AddForce(forceVec, ForceMode.Impulse);
    }
}
