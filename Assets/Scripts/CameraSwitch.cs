﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitch : MonoBehaviour {

    public GameObject cameraOne;
    public GameObject cameraTwo;
    private static bool _mapActive;
    public static event Action<bool> OnMapSwitch;

    //InputActions
    PlayerInputActions _inputAction;

    public InputAction mapAction;

    void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();

        mapAction = _inputAction.Player.Map;
    }

    private void Start()
    {
        //Camera Position Set
        cameraOne.SetActive(true);
        cameraTwo.SetActive(false);
        _mapActive = false;
        //Notify ThirdPersonPlayer on map status
        OnMapSwitch?.Invoke(_mapActive);
    }

    // Update is called once per frame
    private void Update()
    {
        SwitchCamera();
    }

    //Change Camera
    private void SwitchCamera()
    {
        if (mapAction.triggered)
        {
            CameraChangeCounter();
        }

    }

    //Camera Counter
    private void CameraChangeCounter()
    {
        var cameraPositionCounter = PlayerPrefs.GetInt("CameraPosition");
        cameraPositionCounter++;
        CameraPositionChange(cameraPositionCounter);
    }

    //Camera change Logic
    private void CameraPositionChange(int camPosition)
    {
        if (camPosition > 1)
        {
            camPosition = 0;
        }

        //Set camera position database
        PlayerPrefs.SetInt("CameraPosition", camPosition);

        switch (camPosition)
        {
            //Set camera position 1 which is regular camera
            case 0:
                cameraOne.SetActive(true);
                _mapActive = false;
                //Notify ThirdPersonPlayer on map status so player can move
                OnMapSwitch?.Invoke(_mapActive);
                //Time.timeScale = 1f;
                cameraTwo.SetActive(false);
                break;
            //Set camera position 2 which is map camera
            case 1:
                cameraTwo.SetActive(true);
                _mapActive = true;
                //Notify ThirdPersonPlayer on map status so player does not move
                OnMapSwitch?.Invoke(_mapActive);
                //Time.timeScale = 0f;
                cameraOne.SetActive(false);
                break;
        }
    }

    //InputActions
    //Activates all actions in Player action maps (action maps are Player and UI)
    private void OnEnable()
    {
        mapAction.Enable();
        _inputAction.Player.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        mapAction.Disable();
        _inputAction.Player.Disable();
    }

}