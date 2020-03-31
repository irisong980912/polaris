using UnityEngine;

/// <summary>
/// Manages star content
/// </summary>
public class StaticCanvasManager : MonoBehaviour
{
    public GameObject stardustCountText;
    private void Start()
    {
        foreach (Transform child in transform) {
            if (child.gameObject.tag.Contains("|StarButton|")) {
                child.gameObject.SetActive(false);
            }
            
        }
        
        IsometricStarPosManager.OnIsometricStarView += SetIsometricActive;
    }

    private void SetIsometricActive(bool isIsometricActive, Transform star)
    {
        stardustCountText.SetActive(!isIsometricActive);
        
        foreach (Transform child in transform) {
            if (child.gameObject.tag.Contains("|StarButton|")) {
                child.gameObject.SetActive(isIsometricActive);
            }
            
        }

    }
    
    private void OnDisable()
    {
        //Prevent event from looking for prescribed object that is removed on Reload of scene, by unsubscribing.
        IsometricStarPosManager.OnIsometricStarView -= SetIsometricActive;
    }

}