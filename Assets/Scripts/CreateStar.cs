using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreateStar : MonoBehaviour
{
    public AudioSource createSound;
    public AudioSource gravitySound;

    public GameObject createSoundContainer;
    public GameObject gravitySoundContainer;

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
    private PlayerInputActions _inputAction;
    public InputAction interact;

    private void Awake()
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
        
        foreach (var ring in ringAnimationController)
        {
            ring.SetFloat(RingActivationMultiplier, activationSpeedMultiplier);
        }
        foreach (var planet in planetAnimationController)
        {
            planet.SetFloat(PlanetActivationMultiplier, activationSpeedMultiplier);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (!interact.triggered || !enabled) return;
        if (!other.tag.Contains("|Player|")) return;
        if (other.GetComponent<ThirdPersonPlayer>().stardust == 0) return;
        
        FormStar();
    }

    private void FormStar()
    {
        createSound.Play();
        gravitySound.Play();

        GetComponent<Star>().isCreated = true;
        GetComponent<CreateStar>().enabled = false;
        
        OnStarCreation?.Invoke();
        ActivateAnimations();

        // wait until the animations are over
        Invoke(nameof(EnableDestroyStar), 6);
    }

    private void EnableDestroyStar()
    {
        GetComponent<DestroyStar>().enabled = true;
    }

    //Trigger animation clips from animation controller
    private void ActivateAnimations()
    {
        starAnimationController.SetTrigger(PlayCreateStar);
        starVfx.SetTrigger(PlayActivateStarVfx);
       
        foreach (var ring in ringAnimationController)
        {
            ring.SetTrigger(PlayActivateRing);
        } 
        foreach (var planet in planetAnimationController)
        {
            planet.SetTrigger(PlayActivatePlanet);
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
