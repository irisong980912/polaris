using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages star content
/// 
/// </summary>
public class Star : MonoBehaviour
{
    public bool isCreated;
    public float gravityRadius = 180.0f;

    void Start()
    {
        Debug.Log("hi star");
        isCreated = false;
    }


}