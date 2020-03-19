using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages star content
/// 
/// </summary>
public class StaticCanvasManager : MonoBehaviour
{
    private void Start()
    {
        // foreach(Transform child in transform){
        //     if(child.gameObject.tag == "|StarButton|"){
        //         child.gameObject.SetActive(false);
        //     }
        // }
        
        // _isIsometricActive = false;
        IsometricStarView.OnIsometricStarView += SetIsometricActive;
    }

    private void SetIsometricActive(bool isIsometricActive)
    {
        var StardustCountText = transform.Find("StardustCountText").gameObject;
        StardustCountText.SetActive(!isIsometricActive);
        
        foreach(Transform child in transform){
            if(child.gameObject.tag == "|StarButton|"){
                child.gameObject.SetActive(isIsometricActive);
            }
        }

    }



}