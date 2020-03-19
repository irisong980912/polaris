using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages star content
/// 
/// </summary>
public class StarIconManager : MonoBehaviour
{
    public bool isCreated;
    public float gravityRadius = 180.0f;

    void Start()
    {
        isCreated = false;
    }

    // private void Update()
    // {
    //     stardustCount.text = "Stardust: " + stardust;
    // }
}