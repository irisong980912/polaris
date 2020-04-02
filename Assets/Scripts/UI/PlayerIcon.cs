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
    public Transform cam;
    private static bool _mapActive;
    private bool _onSlingShot;
    private bool _isoActive;

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
        transform.LookAt(cam);
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
            if (_isoActive)
            {
                EnableImage();
            }
        }
        else
        {
            Invoke(nameof(EnableImage), 0.0f);
        }
    }
    
    private void SetIsometricActive(bool isoActive, Transform star)
    {
        print("player icon iso view");
        _isoActive = isoActive;
        if (!isoActive)
        {
            print("player icon -- hide");
            GetComponent<Image>().enabled = false;
        }
        else
        {
            Invoke(nameof(EnableImage), 0.0f);
        }
        
    }

    private void EnableImage()
    {
        print("player icon -- show");
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