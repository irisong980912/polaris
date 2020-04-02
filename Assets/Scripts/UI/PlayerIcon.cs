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
    private static bool _mapActive;
    private bool _onSlingShot;

    private void Start()
    {
        
        CameraSwitch.OnMapSwitch += SetMapActive;
        IsometricStarPosManager.OnIsometricStarView += SetIsometricActive;
        RidePlanetSlingshot.OnSlingShot += OnSlingShot;
        GetComponent<Image>().enabled = false;

    }
    
    private void FixedUpdate()
    {
        transform.position = player.position;
    }
    
    private void OnSlingShot(bool onSlingShot, Transform _starToGo)
    {
        GetComponent<Image>().enabled = false;
    }


    private void SetMapActive(bool mapActive)
    {
        _mapActive = mapActive;

        if (!mapActive)
        {
            print("player icon -- hide");
            GetComponent<Image>().enabled = false;
        }
        else
        {
            Invoke(nameof(EnableImage), 2.0f);
        }
    }
    
    private void SetIsometricActive(bool isoActive, Transform star)
    {
        print("player icon iso view");

        if (!isoActive)
        {
            print("player icon -- hide");
            GetComponent<Image>().enabled = false;
        }
        else
        {
            Invoke(nameof(EnableImage), 2.0f);
        }
        
    }

    private void EnableImage()
    {
        GetComponent<Image>().enabled = true;
        GetComponent<Image>().rectTransform.sizeDelta = _mapActive ? new Vector2(2000, 2000) : new Vector2(200, 200);

    }

    private void OnDisable()
    {
        //Prevent event from looking for prescribed object that is removed on Reload of scene, by unsubscribing.
        CameraSwitch.OnMapSwitch -= SetMapActive;
        IsometricStarPosManager.OnIsometricStarView -= SetIsometricActive;
        RidePlanetSlingshot.OnSlingShot -= OnSlingShot;
    }

}