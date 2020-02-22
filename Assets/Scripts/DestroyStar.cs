using System;
using System.Collections.Generic;
using UnityEngine;

public class DestroyStar : MonoBehaviour
{
    public AudioSource destroySound;
    public GameObject destroySoundContainer;

    public AudioSource disperseDustSound;
    public GameObject disperseDustSoundContainer;

    private Collider _other;

    public bool onTrigger;

    public List<GameObject> usedStardust = new List<GameObject>();

    [SerializeField] private Animator starAnimationController;
    [SerializeField] private Animator starVfx;
    [SerializeField] private Animator[] ringAnimationController;

    //Speed multiplier for animations
    public float activationSpeedMultiplier = 1;
    private static readonly int StarActivationMultiplier = Animator.StringToHash("StarActivationMultiplier");
    private static readonly int VfxActivationMultiplier = Animator.StringToHash("VFXActivationMultiplier");
    private static readonly int RingActivationMultiplier = Animator.StringToHash("RingActivationMultiplier");
    private static readonly int PlayDeactivateStar = Animator.StringToHash("playDeactivateStar");
    private static readonly int PlayDeactivateStarVfx = Animator.StringToHash("playDeactivateStarVFX");
    private static readonly int PlayDeactivateRing = Animator.StringToHash("PlayDeactivateRing");

    public static event Action OnStarDestruction; 

    private void Start()
    {
        destroySound = destroySoundContainer.GetComponent<AudioSource>();
        disperseDustSound = disperseDustSoundContainer.GetComponent<AudioSource>();

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
        print("player trigger destroy");
    }


    private void ScatterStar()
    {
        var player = _other.GetComponent<ThirdPersonPlayer>();

        destroySound.Play();
        disperseDustSound.Play();

        var stardust = usedStardust[0];
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

        Invoke(nameof(StartCreate), 4);
    }

    /// <summary>
    /// Will be called by Invoke to set a timeout.
    /// </summary>
    private void StartCreate()
    {
        Debug.Log("StartCreate");
        OnStarDestruction?.Invoke();
        GetComponent<CreateStar>().enabled = true;
    }

    //Trigger animation clips from animation controller
    private void ActivateAnimations()
    {
        starAnimationController.SetTrigger(PlayDeactivateStar);
        starVfx.SetTrigger(PlayDeactivateStarVfx);

        //Iterate through rings array
        foreach (var ring in ringAnimationController)
        {
            ring.SetTrigger(PlayDeactivateRing);
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
