using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DestroyStar : MonoBehaviour
{
    public GameObject star;
    public Material destroyTexture;
    public bool onTrigger;

    // OnTriggerStay is called every physics update a GameObject that has a RigidBody is in the collider.
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown("e"))
        {
            onTrigger = !onTrigger;
        }
    }   

    //TODO: Scatter stardust, change star material.
    //TODO: Later, call animation to destroy star, play SFX.
    private void ScatterStar()
    {
        star.GetComponent<Gravity>().enabled = false;
        star.GetComponent<CreateStar>().enabled = true;
        star.GetComponent<DestroyStar>().enabled = false;
        ChangeMaterial();
    }
    
    // creates a new material instance that looks like the old material
    void ChangeMaterial()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = destroyTexture;
    }

    private void Update()
    {
        if (onTrigger == false){
            ScatterStar();
            print(star.GetComponent<Gravity>().enabled);
            print("destroy" + star.name);
        }
        
    }
}
