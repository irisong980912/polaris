using System.Collections.Generic;
using UnityEngine;

public class DestroyStar : MonoBehaviour
{
    public AudioSource destroySound;

    public GameObject destroySoundContainer;

    public AudioSource disperseDustSound;

    public GameObject disperseDustSoundContainer;

    private Collider _other = null;

    public bool onTrigger;

    public List<GameObject> usedStardust = new List<GameObject>();

    [SerializeField] private Animator StarAnimationController;
    [SerializeField] private Animator StarVFX;
    [SerializeField] private Animator[] RingAnimationController;

    private void Start()
    {
        destroySound = destroySoundContainer.GetComponent<AudioSource>();
        disperseDustSound = disperseDustSoundContainer.GetComponent<AudioSource>();
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
                print("player trigger destroy");

            }

        }
    }


    private void ScatterStar()
    {
        ThirdPersonPlayer player = _other.GetComponent<ThirdPersonPlayer>();

        destroySound.Play();
        disperseDustSound.Play();

        GameObject stardust = usedStardust[0];
        stardust.SetActive(true);
        usedStardust.RemoveAt(0);
        player.litStarNum -= 1;

        // error prevention
        Debug.Log("=== After Destroy ===: " + player.litStarNum);

        GetComponent<Orbit>().enabled = false;
        GetComponent<Gravity>().enabled = false;
        GetComponent<DestroyStar>().enabled = false;

        onTrigger = false;

        ActivateAnimations();

        Invoke("StartCreate", 4);
    }

    /// <summary>
    /// Will be called by Invoke to set a timeout.
    /// </summary>
    void StartCreate()
    {
        Debug.Log("StartCreate");
        GetComponent<CreateStar>().enabled = true;
    }

    //Trigger animation clips from animation controller
    void ActivateAnimations()
    {
        StarAnimationController.SetTrigger("playDeactivateStar");
        StarVFX.SetTrigger("playDeactivateStarVFX");

        //Iterate through rings array
        foreach (Animator ring in RingAnimationController)
        {
            ring.SetTrigger("PlayDeactivateRing");
        }
    }

    private void Update()
    {
        if (onTrigger && _other)
        {

            ScatterStar();
        
        }

    }

}