using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DestroyStar : MonoBehaviour
{

    private Collider _other = null;

    public bool onTrigger;

    public List<GameObject> usedStardust = new List<GameObject>();

    [SerializeField] private Animator StarAnimationController;
    [SerializeField] private Animator StarVFX;

    // OnTriggerStay is called every physics update a GameObject that has a RigidBody is in the collider.
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown("e") && enabled)
        {
            
            if (other.CompareTag("|Player|"))
            {
                onTrigger = true;
                _other = other;
                print("player trigger destroy");

            }

        }
    }


    private void ScatterStar()
    {
        GameObject stardust = usedStardust[0];
        stardust.SetActive(true);
        usedStardust.RemoveAt(0);

        GetComponent<Orbit>().enabled = false;
        GetComponent<Gravity>().enabled = false;

        GetComponent<DestroyStar>().enabled = false;
        GetComponent<CreateStar>().enabled = true;

        onTrigger = false;

        ActivateAnimations();
    }


    //Trigger animation clips from animation controller
    void ActivateAnimations()
    {
        StarAnimationController.SetTrigger("playDeactivateStar");
        StarVFX.SetTrigger("playDeactivateStarVFX");
    }

    private void Update()
    {
        if (onTrigger && _other)
        {

            ScatterStar();
        
        }

    }

}