﻿using UnityEngine;

/// <summary>
/// When a player collides with an asteroid belt, reverses the player's direction of travel, but maintains their speed.
/// </summary>
public class AsteroidBeltBounce : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Rigidbody>().velocity = -other.gameObject.GetComponent<Rigidbody>().velocity;
    }
}
