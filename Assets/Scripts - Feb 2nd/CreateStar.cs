using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class CreateStar : MonoBehaviour
{
    public GameObject star;

    public Transform character;
    private Collider _other = null;
    public bool onTrigger;

    [SerializeField] private Animator StarAnimationController;
    [SerializeField] private Animator StarVFX;

    // OnTriggerStay is called every physics update a GameObject that has a RigidBody is in the collider.
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown("e") && enabled)
        {
            
            if (other.CompareTag("|Player|"))
            {
                // only trigger if the other is player
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
            print("!!!!!!!!!!!!!! has star dust");
            GameObject stardust = player.inventory[0];
            GetComponent<DestroyStar>().usedStardust.Add(stardust);
            player.inventory.RemoveAt(0);

            player.stardust -= 1;

            GetComponent<Orbit>().enabled = true;
            star.GetComponent<Gravity>().enabled = true;

            star.GetComponent<DestroyStar>().enabled = true;
            star.GetComponent<CreateStar>().enabled = false;

            onTrigger = false;

            ActivateAnimations();

        } else
        {
            onTrigger = false;
        }



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
}