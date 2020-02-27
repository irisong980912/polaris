﻿using System;
 using UnityEngine;

 public class CameraSwitch : MonoBehaviour {

    public GameObject playerCamera;
    public GameObject mapCamera;
    private IsometricCamera _camHandler;
    private static bool _mapActive;

    private bool MapActive => _camHandler.IsometricCameraActive;
    public static event Action<bool> OnMapSwitch;
    
    private void Start()
    {
        _camHandler = new IsometricCamera(playerCamera, mapCamera);
        
        //Notify ThirdPersonPlayer on map status
        OnMapSwitch?.Invoke(MapActive);
    }

    // Update is called once per frame
    private void Update()
    {
        SwitchPerspective();
    }

    //Change Camera
    private void SwitchPerspective()
    {
        if (!Input.GetButtonDown("Fire3")) return;
        _camHandler.SwitchPerspective();
        OnMapSwitch?.Invoke(MapActive);
    }

 }
