using System;
using UnityEngine;


/// <summary>
/// Apply this to every planet core
/// change to Isometric view when player is riding a planet
/// </summary>
public class IsometricStarView : MonoBehaviour
{
    
    // TODO: change the naming to enter planet orbit
    // star view pos to planet view pos
    public Transform isometricStarViewPos;

    public Transform cam;
    private bool _isIsometricStarView;
    public static event Action<bool> OnIsometricStarView;
    public static event Action<Transform> OnInitiatePointerToAdjacentStars;

    private void OnTriggerEnter(Collider c)
    {
        if (!transform.parent.parent) return; 
        if (!transform.parent.parent.tag.Contains("|Star|")) return; 
        if (!transform.parent.parent.GetComponent<Star>().isCreated) return;
        if (!c.tag.Contains("|Player|")) return;
        
        OnInitiatePointerToAdjacentStars?.Invoke(transform.parent.parent);

        cam.GetComponent<ThirdPersonCamera>().isometricStarViewPos = isometricStarViewPos;
        
        // give time for camera to pan
        Invoke(nameof(StartIsoView), 0.5f);
    }
    
    private void OnTriggerExit(Collider c)
    {
        if (!c.tag.Contains("|Player|")) return;
        Debug.Log("exit star planet field ");
        
        _isIsometricStarView = false;
        OnIsometricStarView?.Invoke(_isIsometricStarView);
    }
    
    private void StartIsoView()
    {
        Debug.Log("enter star 1 gravity field ");

        _isIsometricStarView = true;
        OnIsometricStarView?.Invoke(_isIsometricStarView);
        
    }
}