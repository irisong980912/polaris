<<<<<<< HEAD:Assets/Scripts - Feb 2nd/CreateStar.cs
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class CreateStar : MonoBehaviour
{
    public AudioSource createSound;
    public AudioSource gravitySound;

    public GameObject createSoundContainer;
    public GameObject gravitySoundContainer;

    public Transform character;
    private Collider _other = null;
    public bool onTrigger;

    [SerializeField] private Animator StarAnimationController;
    [SerializeField] private Animator StarVFX;


    private void Start()
    {

        createSound = createSoundContainer.GetComponent<AudioSource>();
        gravitySound = gravitySoundContainer.GetComponent<AudioSource>();
    }

    // OnTriggerStay is called every physics update a GameObject that has a RigidBody is in the collider.
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButton("Fire2") && enabled)
        {
            
            if (other.CompareTag("|Player|"))
            {
                onTrigger = true;
                _other = other;
                print("player trigger create");

            }
        }
    }

    private void FormStar()
    {

        ThirdPersonPlayer player = _other.GetComponent<ThirdPersonPlayer>();

        if (player.stardust > 0)
        {
            createSound.Play();

            gravitySound.Play();

            print("!!!!!!!!!!!!!! has star dust");
            GameObject stardust = player.inventory[0];
            GetComponent<DestroyStar>().usedStardust.Add(stardust);
            player.inventory.RemoveAt(0);

            player.stardust -= 1;

            GetComponent<Orbit>().enabled = true;
            GetComponent<Gravity>().enabled = true;
            GetComponent<CreateStar>().enabled = false;
            
            onTrigger = false;

            ActivateAnimations();

            // wait untill the animations are over
            Invoke("StartDestroy", 4);

        } else
        {
            onTrigger = false;
        }

    }

    /// <summary>
    /// Will be called by Invoke to set a timeout.
    /// </summary>
    void StartDestroy()
    {
        Debug.Log("StartDestroy");
        GetComponent<DestroyStar>().enabled = true;
    }

    //Trigger animation clips from animation controller
    void ActivateAnimations()
    {
        StarAnimationController.SetTrigger("playCreateStar");
        StarVFX.SetTrigger("playActivateStarVFX");
    }

    private void Update()
    {
        if (onTrigger && _other)
        {
            
            FormStar();

        }

    }
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class CreateStar : MonoBehaviour
{
    public AudioSource createSound;
    public AudioSource gravitySound;

    public GameObject createSoundContainer;
    public GameObject gravitySoundContainer;

    public Transform character;
    private Collider _other = null;
    public bool onTrigger;

    [SerializeField] private Animator StarAnimationController;
    [SerializeField] private Animator StarVFX;


    private void Start()
    {

        createSound = createSoundContainer.GetComponent<AudioSource>();
        gravitySound = gravitySoundContainer.GetComponent<AudioSource>();
    }

    // OnTriggerStay is called every physics update a GameObject that has a RigidBody is in the collider.
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButton("Fire1") && enabled)
        {
            
            if (other.CompareTag("|Player|"))
            {
                onTrigger = true;
                _other = other;
                print("player trigger create");

            }
        }
    }

    private void FormStar()
    {

        ThirdPersonPlayer player = _other.GetComponent<ThirdPersonPlayer>();

        if (player.stardust > 0)
        {
            createSound.Play();

            gravitySound.Play();

            print("!!!!!!!!!!!!!! has star dust");
            GameObject stardust = player.inventory[0];
            GetComponent<DestroyStar>().usedStardust.Add(stardust);
            player.inventory.RemoveAt(0);

            player.stardust -= 1;

            GetComponent<Orbit>().enabled = true;
            GetComponent<Gravity>().enabled = true;
            GetComponent<CreateStar>().enabled = false;
            
            onTrigger = false;

            ActivateAnimations();

            // wait untill the animations are over
            Invoke("StartDestroy", 4);

        } else
        {
            onTrigger = false;
        }

    }

    /// <summary>
    /// Will be called by Invoke to set a timeout.
    /// </summary>
    void StartDestroy()
    {
        Debug.Log("StartDestroy");
        GetComponent<DestroyStar>().enabled = true;
    }

    //Trigger animation clips from animation controller
    void ActivateAnimations()
    {
        StarAnimationController.SetTrigger("playCreateStar");
        StarVFX.SetTrigger("playActivateStarVFX");
    }

    private void Update()
    {
        if (onTrigger && _other)
        {
            
            FormStar();

        }

    }
>>>>>>> c1d2d89f09433d295a2cb0fedf13d89680ce043f:Assets/Scripts/CreateStar.cs
}