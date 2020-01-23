using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DestroyStar : MonoBehaviour
{
    public GameObject star;
    public Material destroyTexture;

    // OnTriggerStay is called every physics update a GameObject that has a RigidBody is in the collider.
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown("e"))
        {
            ScatterStar();
            print("destroy" + star.name);
            ChangeMaterial();
        }
    }

    //TODO: Scatter stardust, change star material.
    //TODO: Later, call animation to destroy star, play SFX.
    private void ScatterStar()
    {
        star.GetComponent<Gravity>().enabled = false;
        star.GetComponent<CreateStar>().enabled = true;
        star.GetComponent<DestroyStar>().enabled = false;
    }
    
    // creates a new material instance that looks like the old material
    void ChangeMaterial()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = destroyTexture;
    }
}
