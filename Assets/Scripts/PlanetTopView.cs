using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// reveal the top view of the planet that the player is riding
/// listening to orbit
/// </summary>
public class PlanetTopView : MonoBehaviour
{
    public Transform planetToTarget;
    public Transform player;

    public float yDisToPlanet = 500.0f;

    public float xDisToPlanet = 100.0f;
    public float zDisToPlanet = 20.0f;

    private bool _startOrbit;
    
    // when player is orbiting the planet
    private void Start()
    {
        _startOrbit = false;
        Orbit.OnOrbitStart += OnOrbitStart;
        Orbit.OnOrbitStop += OnOrbitStop;
    }

    private void OnOrbitStart()
    {
        _startOrbit = true;
    }
    
    private void OnOrbitStop()
    {
        _startOrbit = false;
    }
    
    private void FixedUpdate()
    {
        if (!_startOrbit) return;
        if (!player.parent) return;
        if (!player.parent.tag.Contains("|PlanetCore|")) return;
        planetToTarget = player.parent;
        var dir = new Vector3(xDisToPlanet, yDisToPlanet, zDisToPlanet);
        transform.position = planetToTarget.position + dir;

    }

}