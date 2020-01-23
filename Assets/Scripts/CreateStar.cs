using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStar : MonoBehaviour
{
    public GameObject star;
    public Material newTexture;
    public bool onTrigger;


    // OnTriggerStay is called every physics update a GameObject that has a RigidBody is in the collider.
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown("e"))
        {
            onTrigger = !onTrigger;
        }
    }
    
    private void FormStar()
    {
        star.GetComponent<Gravity>().enabled = true;
        star.GetComponent<DestroyStar>().enabled = true;
        star.GetComponent<CreateStar>().enabled = false;
        ChangeMaterial();
    }

    // creates a new material instance that looks like the old material
    void ChangeMaterial()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = newTexture;
    }
    
    private void Update()
    {
        if (onTrigger == true){
            FormStar();
            print(star.GetComponent<Gravity>().enabled);
            print("create" + star.name);
        }
        
    }
    //TODO: Consume stardust, change star material.
    
    //TODO: Later, call animation to create star, play SFX.
    
}