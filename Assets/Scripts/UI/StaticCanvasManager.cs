using UnityEngine;

/// <summary>
/// Manages star content
/// 
/// </summary>
public class StaticCanvasManager : MonoBehaviour
{
    public GameObject StardustCountText;
    private bool _isEnd;

    private void Start()
    {
        foreach(Transform child in transform){
            if(child.gameObject.tag == "|StarButton|"){
                child.gameObject.SetActive(false);
            }
        }

        RidePlanetSlingshot.OnSlingShot += OnSlingShot;
        RidePlanetSlingshot.OnRidePlanet += OnRidePlanet;
        IsometricStarPosManager.OnIsometricStarView += SetIsometricActive;
        CameraSwitch.OnMapSwitch += SetMapActive;
        // ThirdPersonPlayer.EndSlingShot += EndSlingShot;
    }

    // private void EndSlingShot(bool obj)
    // {
    //     _isEnd = true;
    //     SetStarBtnInactive();
    // }

    private void DisableAllUI(bool onAction)
    {
        StardustCountText.SetActive(!onAction);
        foreach(Transform child in transform){
            if(child.gameObject.tag == "|StarButton|"){
                child.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// When riding the planet, show the HUD for selecting stars
    /// </summary>
    private void OnRidePlanet(bool isOnPlanet)
    {
        StardustCountText.SetActive(false);

        int count = 0;
        foreach(Transform child in transform){
            if(child.gameObject.tag == "|StarButton|")
            {
                count++;
                if (transform.GetComponent<FindAdjacentStars>().adjacentStarNum >= count)
                {
                    child.gameObject.SetActive(isOnPlanet);
                }
                
            }
        }
        // if (isOnPlanet)
        // {
        //     Invoke(nameof(SetStarBtnActive), 4.0f);
        // }
        // else
        // {
        //     Invoke(nameof(SetStarBtnInactive), 0f);
        //     //
        //     
        //     Invoke(nameof(SetStarBtnInactive), 1.0f);
        //     
        //     Invoke(nameof(SetStarBtnInactive), 2.0f);
        //     
        //     Invoke(nameof(SetStarBtnInactive), 3.0f);
        //
        //     if (_isEnd)
        //     {
        //         Invoke(nameof(SetStarBtnInactive), 4.0f);
        //         Invoke(nameof(SetStarBtnInactive), 4.5f);       
        //         Invoke(nameof(SetStarBtnInactive), 5.0f);
        //         Invoke(nameof(SetStarBtnInactive), 5.5f);
        //         Invoke(nameof(SetStarBtnInactive), 6.0f);
        //         Invoke(nameof(SetStarBtnInactive), 6.5f);
        //         _isEnd = false;
        //     }
        //     //
        //     
        // }
        
    }

    // private void SetStarBtnActive()
    // {
    //     int count = 0;
    //     foreach(Transform child in transform){
    //         if(child.gameObject.tag == "|StarButton|")
    //         {
    //             count++;
    //             if (transform.GetComponent<FindAdjacentStars>().adjacentStarNum >= count)
    //             {
    //                 child.gameObject.SetActive(true);
    //             }
    //             
    //         }
    //     }
    // }
    //
    // private void SetStarBtnInactive()
    // {
    //     int count = 0;
    //     foreach(Transform child in transform){
    //         if(child.gameObject.tag == "|StarButton|")
    //         {
    //             count++;
    //             if (transform.GetComponent<FindAdjacentStars>().adjacentStarNum >= count)
    //             {
    //                 child.gameObject.SetActive(false);
    //             }
    //             
    //         }
    //     }
    // }

    /// <summary>
    /// When on the map view, disable all the UI
    /// </summary>
    private void SetMapActive(bool isMapActive)
    {
        DisableAllUI(isMapActive);
    }

    /// <summary>
    /// when performing slingshot, disable all the UI
    /// </summary>
    private void OnSlingShot(bool isOnSlingshot, Transform targetStar)
    {
        DisableAllUI(isOnSlingshot);

    }

    /// <summary>
    /// When on isometric view, disable all the UI
    /// </summary>
    private void SetIsometricActive(bool isIsometricActive, Transform targetStar)
    {
        DisableAllUI(isIsometricActive);

    }
    private void OnDisable()
    {
        //Prevent event from looking for prescribed object that is removed on Reload of scene, by unsubscribing.
        IsometricStarPosManager.OnIsometricStarView -= SetIsometricActive;
        RidePlanetSlingshot.OnSlingShot -= OnSlingShot;
    }

}