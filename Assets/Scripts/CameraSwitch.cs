﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : Subject {

    public GameObject cameraOne;
    public GameObject cameraTwo;
    private static bool _mapActive;
    List<Observer> observers = new List<Observer>();

    // Use this for initialization
    private void Start()
    {
        //Camera Position Set
        cameraOne.SetActive(true);
        cameraTwo.SetActive(false);
        _mapActive = false;
        //Notify ThirdPersonPlayer on map status
        Notify(_mapActive, NotificationType.MapStatus);
    }

    // Update is called once per frame
    private void Update()
    {
        SwitchCamera();
    }

    //Change Camera
    private void SwitchCamera()
    {
        if (Input.GetButtonDown("Fire3"))
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
                Notify(_mapActive, NotificationType.MapStatus);
                //Time.timeScale = 1f;
                cameraTwo.SetActive(false);
                break;
            //Set camera position 2 which is map camera
            case 1:
                cameraTwo.SetActive(true);
                _mapActive = true;
                //Notify ThirdPersonPlayer on map status so player does not move
                Notify(_mapActive, NotificationType.MapStatus);
                //Time.timeScale = 0f;
                cameraOne.SetActive(false);
                break;
        }
    }
}