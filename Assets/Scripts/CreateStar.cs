using System;
using UnityEngine;

public class CreateStar : MonoBehaviour
{
    public AudioSource createSound;
    public AudioSource gravitySound;

    public GameObject createSoundContainer;
    public GameObject gravitySoundContainer;
    
    private Collider _other;
    public bool onTrigger;

    [SerializeField] private Animator starAnimationController;
    [SerializeField] private Animator starVfx;
    [SerializeField] private Animator[] ringAnimationController;

    //Speed multiplier for animations
    public float activationSpeedMultiplier = 1;
    private static readonly int StarActivationMultiplier = Animator.StringToHash("StarActivationMultiplier");
    private static readonly int VfxActivationMultiplier = Animator.StringToHash("VFXActivationMultiplier");
    private static readonly int RingActivationMultiplier = Animator.StringToHash("RingActivationMultiplier");
    private static readonly int PlayCreateStar = Animator.StringToHash("playCreateStar");
    private static readonly int PlayActivateStarVfx = Animator.StringToHash("playActivateStarVFX");
    private static readonly int PlayActivateRing = Animator.StringToHash("PlayActivateRing");

    public static event Action OnStarCreation; 

    private void Start()
    {
        createSound = createSoundContainer.GetComponent<AudioSource>();
        gravitySound = gravitySoundContainer.GetComponent<AudioSource>();

        starAnimationController.SetFloat(StarActivationMultiplier, activationSpeedMultiplier);
        starVfx.SetFloat(VfxActivationMultiplier, activationSpeedMultiplier);

        //Iterate through rings array
        foreach (var ring in ringAnimationController)
        {
            ring.SetFloat(RingActivationMultiplier, activationSpeedMultiplier);
        }
    }

    // OnTriggerStay is called every physics update a GameObject that has a RigidBody is in the collider.
    private void OnTriggerStay(Collider other)
    {
        if (!Input.GetButton("Fire2") || !enabled) return;
        if (!other.tag.Contains("|Player|")) return;
        onTrigger = true;
        _other = other;
        print("player trigger create");
    }

    private void FormStar()
    {
        var player = _other.GetComponent<ThirdPersonPlayer>();

        if (player.stardust > 0)
        {
            createSound.Play();

            gravitySound.Play();

            print("!!!!!!!!!!!!!! has star dust");

            // update stardust num and the lit star num
            var stardust = player.inventory[player.stardustSelection];
            GetComponent<DestroyStar>().usedStardust.Add(stardust);
            player.inventory.RemoveAt(player.stardustSelection);
            player.stardust -= 1;

            transform.Find("GravityCore").GetComponent<Orbit>().enabled = true;
            transform.Find("GravityCore").GetComponent<Gravity>().enabled = true;

            // enable the orbit script of all planets of the star

            //GetComponentInChildren<Gravity>().enabled = true;
            GetComponent<CreateStar>().enabled = false;
            
            onTrigger = false;

            ActivateAnimations();

            // wait until the animations are over
            Invoke(nameof(StartDestroy), 6);

            

        } else
        {
            onTrigger = false;
        }

    }

    /// <summary>
    /// Will be called by Invoke to set a timeout.
    /// </summary>
    private void StartDestroy()
    {
        Debug.Log("StartDestroy");
        OnStarCreation?.Invoke();
        GetComponent<DestroyStar>().enabled = true;
    }

    //Trigger animation clips from animation controller
    private void ActivateAnimations()
    {
        starAnimationController.SetTrigger(PlayCreateStar);
        starVfx.SetTrigger(PlayActivateStarVfx);
       
        //Iterate through rings array 
        foreach (var ring in ringAnimationController)
        {
            ring.SetTrigger(PlayActivateRing);
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