﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : Subject {

    public GameObject cameraOne;
    public GameObject cameraTwo;
    public static bool MapActive;
    List<Observer> observers = new List<Observer>();

    // Use this for initialization
    void Start()
    {
        //Camera Position Set
        cameraOne.active = true;
        cameraTwo.active = false;
        MapActive = false;
        //Notify ThirdPersonPlayer on map status
        Notify(MapActive, NotificationType.MapStatus);
    }

    // Update is called once per frame
    void Update()
    {
        switchCamera();
    }

    //Change Camera
    void switchCamera()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            cameraChangeCounter();
        }
    }

    //Camera Counter
    void cameraChangeCounter()
    {
        int cameraPositionCounter = PlayerPrefs.GetInt("CameraPosition");
        cameraPositionCounter++;
        cameraPositionChange(cameraPositionCounter);
    }

    //Camera change Logic
    void cameraPositionChange(int camPosition)
    {
        if (camPosition > 1)
        {
            camPosition = 0;
        }

        //Set camera position database
        PlayerPrefs.SetInt("CameraPosition", camPosition);

        //Set camera position 1 which is regular camera
        if (camPosition == 0)
        {
            cameraOne.SetActive(true);
            MapActive = false;
            //Notify ThirdPersonPlayer on map status so player can move
            Notify(MapActive, NotificationType.MapStatus);
            //Time.timeScale = 1f;
            cameraTwo.SetActive(false);
        }

        //Set camera position 2 which is map camera
        if (camPosition == 1)
        {
            cameraTwo.SetActive(true);
            MapActive = true;
            //Notify ThirdPersonPlayer on map status so player does not move
            Notify(MapActive, NotificationType.MapStatus);
            //Time.timeScale = 0f;
            cameraOne.SetActive(false);
        }

    }
}