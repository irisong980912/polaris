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
    [SerializeField] private Animator[] RingAnimationController;

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

            Debug.Log("=== Before creation ===: " + player.litStarNum);

            // update stardust num and the lit star num
            GameObject stardust = player.inventory[player.stardustSelection];
            GetComponent<DestroyStar>().usedStardust.Add(stardust);
            player.inventory.RemoveAt(player.stardustSelection);
            player.stardust -= 1;
            player.litStarNum += 1;

            Debug.Log("=== After creation ===: " + player.litStarNum);

            GetComponent<Orbit>().enabled = true;
            GetComponent<Gravity>().enabled = true;
            GetComponent<CreateStar>().enabled = false;
            
            onTrigger = false;

            ActivateAnimations();

            // wait untill the animations are over
            Invoke("StartDestroy", 4);

            // check if all the stars has 

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
       
        //Iterate through rings array 
        foreach (Animator ring in RingAnimationController)
        {
            ring.SetTrigger("PlayActivateRing");
        }
    }

    private void Update()
    {
        if (onTrigger && _other)
        {
            
            FormStar();

        }

    }
}