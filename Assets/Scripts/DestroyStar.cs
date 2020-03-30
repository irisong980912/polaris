using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private Animator[] planetAnimationController;


    //Speed multiplier for animations
    public float activationSpeedMultiplier = 1;
    private static readonly int StarActivationMultiplier = Animator.StringToHash("StarActivationMultiplier");
    private static readonly int VfxActivationMultiplier = Animator.StringToHash("VFXActivationMultiplier");
    private static readonly int RingActivationMultiplier = Animator.StringToHash("RingActivationMultiplier");
    private static readonly int PlanetActivationMultiplier = Animator.StringToHash("PlanetActivationMultiplier");

    private static readonly int PlayDeactivateStar = Animator.StringToHash("playDeactivateStar");
    private static readonly int PlayDeactivateStarVfx = Animator.StringToHash("playDeactivateStarVFX");
    private static readonly int PlayDeactivateRing = Animator.StringToHash("PlayDeactivateRing");
    private static readonly int PlayDeactivatePlanet = Animator.StringToHash("PlayDeactivatePlanet");

    public static event Action OnStarDestruction;

    //InputActions
    PlayerInputActions inputAction;
    
    public InputAction Interact;

    void Awake()
    {
        //InputActions
        inputAction = new PlayerInputActions();
        Interact = inputAction.Player.Interact;

    }

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

        //Iterate through rings array
        foreach (var planet in planetAnimationController)
        {
            planet.SetFloat(PlanetActivationMultiplier, activationSpeedMultiplier);
        }
    }

    // OnTriggerStay is called every physics update a GameObject that has a RigidBody is in the collider.
    private void OnTriggerStay(Collider other)
    {
        //if (!Input.GetButton("Fire2") || !enabled) return;

        //InputAction replaces "Input.GetButton("Example") and holds a bool
        if (!Interact.triggered || !enabled) return;

        if (!other.tag.Contains("|Player|")) return;
        onTrigger = true;
        _other = other;
        print("player trigger destroy");
    }


    private void ScatterStar()
    {
        destroySound.Play();
        disperseDustSound.Play();
        var player = _other.GetComponent<ThirdPersonPlayer>();
        player.stardust += 1;
        // var stardust = usedStardust[0];
        // stardust.SetActive(true);
        // usedStardust.RemoveAt(0);

        // transform.Find("GravityCore").GetComponent<Orbit>().enabled = false;
        // transform.Find("GravityCore").GetComponent<Gravity>().enabled = false;

        GetComponent<Star>().isCreated = false;

        GetComponent<DestroyStar>().enabled = false;
        
        OnStarDestruction?.Invoke();

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
        // OnStarDestruction?.Invoke();
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

        //Iterate through Planets array 
        foreach (var planet in planetAnimationController)
        {
            planet.SetTrigger(PlayDeactivatePlanet);
        }
    }

    private void Update()
    {
        if (onTrigger && _other)
        {
            ScatterStar();
        }
    }

    //InputActions
    //Activates all actions in Player action maps (action maps are Player and UI)
    private void OnEnable()
    {
        Interact.Enable();
        inputAction.Player.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        Interact.Disable();
        inputAction.Player.Disable();
    }

}
