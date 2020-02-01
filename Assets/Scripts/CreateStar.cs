using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class CreateStar : MonoBehaviour
{
    public GameObject star;
    //public Material newTexture;
    public bool onTrigger;

    [SerializeField] private Animator StarAnimationController;
    [SerializeField] private Animator StarVFX;

    // OnTriggerStay is called every physics update a GameObject that has a RigidBody is in the collider.
    private void OnTriggerStay(Collider collision)
    {
        ThrirdPersonPlayer player = collision.GetComponent<ThrirdPersonPlayer>();
        if (Input.GetKeyDown("e") && player.stardust > 0)
        {
            onTrigger = !onTrigger;
        }
    }

    private void FormStar()
    {
        star.GetComponent<Gravity>().enabled = true;
        star.GetComponent<DestroyStar>().enabled = true;
        star.GetComponent<CreateStar>().enabled = false;
        //ChangeMaterial();

        ActivateAnimations();
    }

    // creates a new material instance that looks like the old material
    /*
    void ChangeMaterial()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = newTexture;
    }
    */

    //Trigger animation clips from animation controller
    void ActivateAnimations()
    {
        StarAnimationController.SetTrigger("playCreateStar");
        StarVFX.SetTrigger("playActivateStarVFX");
    }

    private void Update()
    {
        if (onTrigger == true)
        {
            FormStar();
            print(star.GetComponent<Gravity>().enabled);
            print("create" + star.name);
        }

    }
}