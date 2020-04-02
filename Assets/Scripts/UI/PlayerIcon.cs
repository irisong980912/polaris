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
        
        RidePlanetSlingshot.OnRidePlanet += OnRidePlanet;
        CreateStar.OnStarCreation += OnStarCreation;

    }
    
    private void FixedUpdate()
    {
        
        transform.position = player.position;
        transform.LookAt(cam);
    }
    
    // disable when on cutscene
    private void OnStarCreation()
    {
        GetComponent<Image>().enabled = false;
        Invoke(nameof(FinishStarCreationAnimation), 6.0f);
    }

    private void FinishStarCreationAnimation()
    {
        EnableImage();
    }

    
    private void OnRidePlanet(bool isOnPlanet)
    {
        if (isOnPlanet)
        {
            GetComponent<Image>().enabled = false;
            Invoke(nameof(FinishPlanetAnimation), 6.0f);
        }
    }
    
    private void FinishPlanetAnimation()
    {
        EnableImage();
    }
    
    private void OnSlingShot(bool onSlingShot, Transform _starToGo)
    {
        print("player icon OnSlingShot -- hide");
        GetComponent<Image>().enabled = false;
    }


    private void SetMapActive(bool mapActive)
    {
        _mapActive = mapActive;

        if (!mapActive)
        {
            print("player icon SetMapActive -- hide");
            GetComponent<Image>().enabled = false;
            if (_isoActive)
            {
                EnableImage();
            }
        }
        else
        {
            Invoke(nameof(EnableImage), 1.0f);
        }
    }
    
    private void SetIsometricActive(bool isoActive, Transform star)
    {
        print("player icon iso view");
        _isoActive = isoActive;
        if (!isoActive)
        {
            print("player icon SetIsometricActive -- hide");
            GetComponent<Image>().enabled = false;
        }
        else
        {
            Invoke(nameof(EnableImage), 1.0f);
        }
        
    }

    private void EnableImage()
    {
        print("player icon -- show");
        GetComponent<Image>().enabled = true;
        GetComponent<Image>().rectTransform.sizeDelta = _mapActive ? new Vector2(2000, 2000) : new Vector2(200, 200);
        transform.LookAt(cam);
    }

    private void OnDisable()
    {
        //Prevent event from looking for prescribed object that is removed on Reload of scene, by unsubscribing.
        CameraSwitch.OnMapSwitch -= SetMapActive;
        IsometricStarPosManager.OnIsometricStarView -= SetIsometricActive;
        RidePlanetSlingshot.OnSlingShot -= OnSlingShot;
    }

}