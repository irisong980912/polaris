using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages Player Icon Location
/// Appears when camera switch to map mode and when in isometric view
/// 
/// </summary>
public class PlayerIcon : MonoBehaviour
{
    public Transform player;
    private static bool _isoActive;
    private static bool _mapActive;

    private void Start()
    {
        CameraSwitch.OnMapSwitch += SetMapActive;
        IsometricStarPosManager.OnIsometricStarView += SetIsometricActive;
        StarIconManager.OnSelectStar += OnSlingShot;
        GetComponent<Image>().enabled = false;

    }

    private void OnSlingShot(Transform starToGo)
    {
        _isoActive = false;
        GetComponent<Image>().enabled = false;
    }

    private void FixedUpdate()
    {
        transform.position = player.position;
    }

    private void SetMapActive(bool mapActive)
    {
        _mapActive = mapActive;

        HandleImageEnable();


    }
    
    private void SetIsometricActive(bool isoActive, Transform star)
    {
        _isoActive = isoActive;
        
        Invoke(nameof(HandleImageEnable), 4.0f);
        // HandleImageEnable();
    }

    private void HandleImageEnable()
    {
        if (_mapActive || _isoActive)
        {
            GetComponent<Image>().enabled = true;
            GetComponent<Image>().rectTransform.sizeDelta = _mapActive ? new Vector2(2000, 2000) : new Vector2(200, 200);
        }
        else
        {
            print("player icon -- hide");
            GetComponent<Image>().enabled = false;
        }
    }

    private void OnDisable()
    {
        //Prevent event from looking for prescribed object that is removed on Reload of scene, by unsubscribing.
        CameraSwitch.OnMapSwitch -= SetMapActive;
        IsometricStarPosManager.OnIsometricStarView -= SetIsometricActive;
        StarIconManager.OnSelectStar -= OnSlingShot;
    }

}