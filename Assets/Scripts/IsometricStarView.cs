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
    public static event Action OnEnterGravityField;
    public static event Action OnExitGravityField;
    public Transform isometricStarViewPos;

    public Transform cam;
    private bool _isIsometricStarView;
    public static event Action<bool> OnIsometricStarView;
    public static event Action<Transform> OnInitiatePointerToAdjacentStars;

    private void OnTriggerEnter(Collider c)
    {
        // when star are lit
        if (!transform.parent.parent) return; 
        if (!transform.parent.parent.CompareTag("|Star|")) return; 
        if (!transform.parent.parent.GetComponent<Star>().isCreated) return;
        if (!c.tag.Contains("|Player|")) return;
        
        // print("--------------- iso-planet core parent parent");
        // print(transform.parent.parent.tag);
        OnInitiatePointerToAdjacentStars?.Invoke(transform.parent.parent);

        cam.GetComponent<ThirdPersonCamera>().isometricStarViewPos = isometricStarViewPos;
        Invoke(nameof(EnterField), 0.5f);
    }
    
    private void OnTriggerExit(Collider c)
    {
        if (!c.tag.Contains("|Player|")) return;
        Debug.Log("exit star planet field ");
        
        _isIsometricStarView = false;
        OnIsometricStarView?.Invoke(_isIsometricStarView);
        
        OnExitGravityField?.Invoke();
    
    
    }
    
    private void EnterField()
    {
        Debug.Log("enter star 1 gravity field ");
        OnEnterGravityField?.Invoke();
        
        _isIsometricStarView = true;
        OnIsometricStarView?.Invoke(_isIsometricStarView);
        
    }
}