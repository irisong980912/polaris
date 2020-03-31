using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DestroyStar : MonoBehaviour
{
    public AudioSource destroySound;
    public GameObject destroySoundContainer;

    public AudioSource disperseDustSound;
    public GameObject disperseDustSoundContainer;

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
        destroySound = destroySoundContainer.GetComponent<AudioSource>();
        disperseDustSound = disperseDustSoundContainer.GetComponent<AudioSource>();

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
        ScatterStar();
    }

    private void ScatterStar()
    {
        destroySound.Play();
        disperseDustSound.Play();

        GetComponent<Star>().isCreated = false;
        GetComponent<DestroyStar>().enabled = false;
        
        OnStarDestruction?.Invoke();

        ActivateAnimations();
        Invoke(nameof(EnableCreateStar), 4);
    }

    private void EnableCreateStar()
    {
        GetComponent<CreateStar>().enabled = true;
    }

    //Trigger animation clips from animation controller
    private void ActivateAnimations()
    {
        starAnimationController.SetTrigger(PlayDeactivateStar);
        starVfx.SetTrigger(PlayDeactivateStarVfx);

        foreach (var ring in ringAnimationController)
        {
            ring.SetTrigger(PlayDeactivateRing);
        }
        foreach (var planet in planetAnimationController)
        {
            planet.SetTrigger(PlayDeactivatePlanet);
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
