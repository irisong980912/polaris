using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages star content
/// 
/// </summary>
public class StaticCanvasManager : MonoBehaviour
{


    private void Start()
    {
        IsometricStarView.OnIsometricStarView += SetIsometricActive;
    }

    private void SetIsometricActive(bool isIsometricActive)
    {
        

        var child = transform.Find("StardustCountText").gameObject;
        child.SetActive(!isIsometricActive);
    }



}