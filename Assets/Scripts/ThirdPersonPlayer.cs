﻿using UnityEngine;
using System.Collections.Generic;

public class ThirdPersonPlayer : MonoBehaviour
{

    public float speed;

    public int stardust;
    public List<GameObject> inventory = new List<GameObject>();

    public Transform cam;

    private Rigidbody _body;
    private Vector3 _direction;

    private static bool _mapActive;

    public int stardustSelection;

    public AudioSource collectDustSound;
    public GameObject collectDustSoundContainer;

    private void Start()
    {
        //Get the camera switch script so it can add this as an observer
        try
        {
            var goList = new List<GameObject>();
            foreach (var o in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                var go = (GameObject) o;
                if (go.tag.Contains("|MapScript|"))
                {
                }
            }
        }
        catch (UnityException)
        {
            print("No such tag");
        }
        CameraSwitch.OnMapSwitch += SetMapActive;
    }

    private static void SetMapActive(bool mapActive)
    {
        _mapActive = mapActive;
    }


    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        collectDustSound = collectDustSoundContainer.GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider collision)
    {
        // TODO: Move me to the collect stardust script.
        if (collision.tag.Contains("|Dust|"))
        {
            collectDustSound.Play();
        }
    }


    private void FixedUpdate()
    {

        if (_mapActive == false)
        {
            Move();
        }

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

        _body.transform.Translate(cam.transform.TransformDirection(_direction) * speed, Space.World);
    }


    private void HandleRotation()
    {
        // determines which angle that the camera is looking at
        var targetRotation = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        var lookAt = Quaternion.Slerp(transform.rotation,
                                      Quaternion.Euler(0, targetRotation, 0),
                                      0.5f);
        _body.rotation = lookAt;
    }

}

public enum NotificationType
{
    MapStatus
}
