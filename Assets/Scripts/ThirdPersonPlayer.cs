using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class ThirdPersonPlayer : MonoBehaviour
{

    public float speed;

    public int litStarNum;

    public int stardust;
    public List<GameObject> inventory = new List<GameObject>();

    public Transform cam;

    private Rigidbody _body;

    private Vector3 _direction;

    public int stardustSelection;

    public GameObject inv1;

    public GameObject inv2;

    public AudioSource collectDustSound;
    public GameObject collectDustSoundContainer;

    private void Start()
    {
        litStarNum = 0;
    }

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
