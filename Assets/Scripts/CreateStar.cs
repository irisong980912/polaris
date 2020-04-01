using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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
    [SerializeField] private Animator[] planetAnimationController;


    //Speed multiplier for animations
    public float activationSpeedMultiplier = 1;
    private static readonly int StarActivationMultiplier = Animator.StringToHash("StarActivationMultiplier");
    private static readonly int VfxActivationMultiplier = Animator.StringToHash("VFXActivationMultiplier");
    private static readonly int RingActivationMultiplier = Animator.StringToHash("RingActivationMultiplier");
    private static readonly int PlanetActivationMultiplier = Animator.StringToHash("PlanetActivationMultiplier");


    private static readonly int PlayCreateStar = Animator.StringToHash("playCreateStar");
    private static readonly int PlayActivateStarVfx = Animator.StringToHash("playActivateStarVFX");
    private static readonly int PlayActivateRing = Animator.StringToHash("PlayActivateRing");
    private static readonly int PlayActivatePlanet = Animator.StringToHash("PlayActivatePlanet");

    
    public static event Action OnStarCreation;
    
    //InputActions
    PlayerInputActions _inputAction;

    [FormerlySerializedAs("Interact")] public InputAction interact;

    void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();
        interact = _inputAction.Player.Interact;

    }

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

        //Iterate through planets array
        foreach (var planet in planetAnimationController)
        {
            planet.SetFloat(PlanetActivationMultiplier, activationSpeedMultiplier);
        }

    }

    // OnTriggerStay is called every physics update a GameObject that has a RigidBody is in the collider.
    private void OnTriggerStay(Collider other)
    {
        //if (!Input.GetButton("Fire2") || !enabled) return

        //InputAction replaces "Input.GetButton("Example") and holds a bool
        if (!interact.triggered || !enabled) return;

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
            
            player.stardust -= 1;

            // transform.Find("GravityCore").GetComponent<Orbit>().enabled = true;
            // transform.Find("GravityCore").GetComponent<Gravity>().enabled = true;
            GetComponent<Star>().isCreated = true;
            GetComponent<CreateStar>().enabled = false;
            
            OnStarCreation?.Invoke();
            
            
            onTrigger = false;

            ActivateAnimations();

            // wait until the animations are over
            Invoke(nameof(StartDestroy), 6);
        } 
        else
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

        //Iterate through Planets array 
        foreach (var planet in planetAnimationController)
        {
            planet.SetTrigger(PlayActivatePlanet);
        }

    }

    private void Update()
    {
        if (onTrigger && _other)
        {
            FormStar();
        }
    }

    //InputActions
    //Activates all actions in Player action maps (action maps are Player and UI)
    private void OnEnable()
    {
        interact.Enable();
        _inputAction.Player.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        interact.Disable();
        _inputAction.Player.Disable();
    }

}
