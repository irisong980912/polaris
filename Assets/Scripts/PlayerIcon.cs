using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Manages Player Icon Location
/// Appears when camera switch to map mode and when in isometric view
/// 
/// </summary>
public class PlayerIcon : MonoBehaviour
{
    public Transform player;
    private static bool _isoActive;
    private static bool _mapActive;

    private void Start()
    {
        
        CameraSwitch.OnMapSwitch += SetMapActive;
        IsometricStarView.OnIsometricStarView += SetIsometricActive;
        GetComponent<Image>().enabled = false;

    }
    private void FixedUpdate()
    {
        transform.position = player.position;
    }

    private void SetMapActive(bool mapActive)
    {
        _mapActive = mapActive;

        HandleImageEnable();


    }
    
    private void SetIsometricActive(bool isoActive)
    {
        _isoActive = isoActive;
        
        Invoke(nameof(HandleImageEnable), 4.0f);
        // HandleImageEnable();
    }

    private void HandleImageEnable()
    {
        if (_mapActive || _isoActive)
        {
            GetComponent<Image>().enabled = true;
            GetComponent<Image>().rectTransform.sizeDelta = _mapActive ? new Vector2(2000, 2000) : new Vector2(200, 200);
        }
        else
        {
            print("player icon -- hide");
            GetComponent<Image>().enabled = false;
        }
    }
    
}