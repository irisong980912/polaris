using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ThirdPersonPlayer : MonoBehaviour
{
    public float speed;
    public int stardust;
    public TextMeshProUGUI stardustcount;
    public List<GameObject> inventory = new List<GameObject>();

    public Transform cam;

    private static bool _mapActive;

    public int stardustSelection;

    public AudioSource collectDustSound;
    public GameObject collectDustSoundContainer;

    private void Start()
    {
        CameraSwitch.OnMapSwitch += SetMapActive;
    }

    private static void SetMapActive(bool mapActive)
    {
        _mapActive = mapActive;
    }

    private void Awake()
    {
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
        if (_mapActive) return;
        
        var xAxisInput = Input.GetAxisRaw("Horizontal");
        var yAxisInput = Input.GetAxisRaw("Vertical");

        if (Math.Abs(xAxisInput) < 0.1f && Math.Abs(yAxisInput) < 0.1f) return;
        
        var directionFromInput = new Vector3(xAxisInput, 0f, yAxisInput).normalized;
        var directionOfTravel = cam.TransformDirection(directionFromInput);
        
        transform.Translate(directionOfTravel * speed, Space.World);
        transform.forward = directionOfTravel;
    }

}
