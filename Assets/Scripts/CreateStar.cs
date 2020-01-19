using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStar : MonoBehaviour
{
    public GameObject star;

    // OnTriggerStay is called every physics update a GameObject that has a RigidBody is in the collider.
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown("e"))
        {
            FormStar();
        }
    }

    //TODO: Consume stardust, change star material.
    //TODO: Later, call animation to create star, play SFX.
    private void FormStar()
    {
        star.GetComponent<Gravity>().enabled = true;
        star.GetComponent<DestroyStar>().enabled = true;
        star.GetComponent<CreateStar>().enabled = false;
    }
}