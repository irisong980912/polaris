using UnityEngine;

/// <summary>
/// Manages star content
/// 
/// </summary>
public class StaticCanvasManager : MonoBehaviour
{
    public GameObject StardustCountText;
    private bool _isEnd;
    private bool _isOnPlanet;

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
    }

    private void DisableAllUI(bool onAction)
    {
        StardustCountText.SetActive(!onAction);
        foreach(Transform child in transform){
            if(child.gameObject.tag == "|StarButton|"){
                child.gameObject.SetActive(false);
            }
        }
    }
    
    
    private void OnRidePlanet(bool isOnPlanet)
    {
        _isOnPlanet = isOnPlanet;
        print("static -- OnRidePlanet " + isOnPlanet);
        if (isOnPlanet)
        {
            StardustCountText.SetActive(false);

            Invoke(nameof(ShowSlingBtns), 4);
        }
        else
        {
            foreach(Transform child in transform){
                if (child.gameObject.tag == "|StarButton|")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        
    }
    
    private void ShowSlingBtns()
    {
        
        print("tutorial animate --- ShowSlingText" + _isOnPlanet);

        if (!_isOnPlanet) return;
        int count = 0;
        foreach(Transform child in transform){
            if(child.gameObject.tag == "|StarButton|")
            {
                count++;
                if (transform.GetComponent<FindAdjacentStars>().adjacentStarNum >= count)
                {
                    child.gameObject.SetActive(true);
                }
                    
            }
        }
    }


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