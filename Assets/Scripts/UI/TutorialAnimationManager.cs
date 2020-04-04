using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages Tutorial Animation
/// 1. activate star
/// 2. deactivate star
/// 3. boost when have more than three stars
/// 4. notify to collect stardusts
/// </summary>
public class TutorialAnimationManager : MonoBehaviour
{

    public RectTransform animActivatePS4;
    public RectTransform animDeactivatePS4;
    //public RawImage animSlingshotPS4;

    //public RawImage animActivateXBOX;
    //public RawImage animDeactivateXBOX;
    //public RawImage animSlingshotXBOX;

    public RectTransform animCollectStardust;
    public RectTransform animBoost;
    
    public TextMeshProUGUI tutorialtext;
    
    public Transform player;
    private bool _onPlanet;


    void Start()
    {
        CreateStar.OnStarCreation += OnStarCreation;
        DestroyStar.OnStarDestruction += OnStarDestruction;
        Boost.OnBoost += OnBoost;
        StardustPickup.OnPickingStardusts += OnPickingStardusts;
        IsometricStarPosManager.OnIsometricStarView += OnIsometricStarView;
        RidePlanetSlingshot.OnRidePlanet += OnRidePlanet;

        animActivatePS4.GetComponent<RawImage>().enabled = false;
        animDeactivatePS4.GetComponent<RawImage>().enabled = false;
        animCollectStardust.GetComponent<RawImage>().enabled = false;
        animBoost.GetComponent<RawImage>().enabled = false;
    }

    private void Update()
    {
        // if player is orbiting the star, allow showing animation for create and destroy stars
        if (!player.parent) return;
        if (!player.parent.CompareTag("|GravityCoreForPlayer|")) return;
        AnimActivatePS4();
        AnimDeactivatePS4();
    }
    
    /// <summary>
    /// Show animation of activate star when
    /// 1. player is orbiting the star within a certain distance
    /// 2. star is dark
    /// not
    /// 1. when player is on the planet
    /// 2. showing the map
    /// 3. star is lit
    ///
    /// notify when can show the animation (after the animation of destroy star finishes)
    /// 
    /// </summary>
    private void AnimActivatePS4()
    {
        var curStar = player.parent.parent;
        // if animation of destroy star finishes or not yet animating
        if (!curStar.GetComponent<CreateStar>().enabled) return;

        var playerToStarDis = Vector3.Distance(player.position, curStar.position);
        if (playerToStarDis > 60) return;
        animActivatePS4.GetComponent<RawImage>().enabled = true;
        
        // disable when star is activated or after time out 
        // Invoke(nameof(DisableImage), 15);
    }

    private void AnimDeactivatePS4()
    {
        var curStar = player.parent.parent;
        // if animation of destroy star finishes or not yet animating
        if (!curStar.GetComponent<DestroyStar>().enabled) return;

        var playerToStarDis = Vector3.Distance(player.position, curStar.position);
        if (playerToStarDis > 60) return;
        animDeactivatePS4.GetComponent<RawImage>().enabled = true;
    }
    
    private void OnPickingStardusts(bool isPicking)
    {
        animCollectStardust.GetComponent<RawImage>().enabled = isPicking;
    }

    private void OnBoost(bool obj)
    {
        animBoost.GetComponent<RawImage>().enabled = true;
        // disable when on isometric view
        
        Invoke(nameof(DisableAnimBoost), 10);
    }

    private void DisableAnimBoost()
    {
        animBoost.GetComponent<RawImage>().enabled = false;
    }
    
    private void OnStarCreation()
    {
        // tutorialtext.text = "Star";
        animActivatePS4.GetComponent<RawImage>().enabled = false;
    }
    private void OnStarDestruction()
    {
        animDeactivatePS4.GetComponent<RawImage>().enabled = false;
    }
    
    /// <summary>
    /// Disable all tutorial prompts except star destroy and creation when on isometric view.
    /// </summary>
    private void OnIsometricStarView(bool onIso, Transform arg2)
    {
        if (onIso)
        {
            animBoost.GetComponent<RawImage>().enabled = false;
            animCollectStardust.GetComponent<RawImage>().enabled = false;
        }
        else
        {
            animDeactivatePS4.GetComponent<RawImage>().enabled = false;
            animActivatePS4.GetComponent<RawImage>().enabled = false;
        }
        
    }
    
    private void OnRidePlanet(bool onPlanet)
    {
        print("static -- OnRidePlanet " + onPlanet);
        _onPlanet = onPlanet;
        if (onPlanet)
        {
            animDeactivatePS4.GetComponent<RawImage>().enabled = false;
            animActivatePS4.GetComponent<RawImage>().enabled = false;
            
            Invoke(nameof(ShowSlingText), 4);
        }
        else
        {
            tutorialtext.text = "";
        }
        
    }

    private void ShowSlingText()
    {
        
        print("tutorial animate --- ShowSlingText" + _onPlanet);
        if (!_onPlanet) return;
        tutorialtext.text = "Select a star using Dpad to slingshot";
        
    }

}