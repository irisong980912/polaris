﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThirdPersonPlayer : MonoBehaviour
{

    public float speed = 200f;
    public float circleRadius = 1f;

    public int stardust = 0;
    public List<GameObject> inventory = new List<GameObject>();

    public Transform cam; 

    private Rigidbody _body;

    private Vector3 _direction;

    public AudioSource collectDustSound;
    public GameObject collectDustSoundContainer;

    private bool _inControl;
    
    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        collectDustSound = collectDustSoundContainer.GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("|Dust|"))
        {
            collectDustSound.Play();
        }
    }

    
    private void FixedUpdate()
    {

        Move();

        // if the mc is not at the same location as the camera,
        // handle camera rotation
        if (_direction != Vector3.zero)
        {
            HandleRotation();
        }

    }


    private void Move()
    {
        // x is left and right
        // y is in and out
        var xAxis = Input.GetAxisRaw("Horizontal");
        var yAxis = Input.GetAxisRaw("Vertical");
 
        _direction = new Vector3(xAxis, 0f, yAxis);
        _direction = _direction.normalized;
        
        // Camera.main to cam
        // Convert direction from local to world relative to camera
        
        // Only alters the player's movement when there are inputs;
        // otherwise, player is free to be moved around by forces.
        if (Math.Abs(xAxis) > 0.05 || Math.Abs(yAxis) > 0.05)
        {
            _inControl = true;
            _body.velocity = cam.transform.TransformDirection(_direction) * (speed * Time.deltaTime);
        }
        else
        {
            if (!_inControl || _body.velocity == Vector3.zero) return;
            _body.velocity = Vector3.zero;
            _inControl = false;
        }

    }


    private void HandleRotation()
    {
        // determines which angle that the camera is looking at
        var targetRotation = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        var lookAt = Quaternion.Slerp(transform.rotation,
                                      Quaternion.Euler(0,targetRotation,0),
                                      0.5f);
        _body.rotation = lookAt;
 
    }

}
