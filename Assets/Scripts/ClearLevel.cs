using System;
using UnityEngine;

public class ClearLevel : MonoBehaviour
{
    public GameObject clearLevelImage;
    public GameObject stardustCountText;
    public GameObject tutorialZoneText;
    
    public int totalStarNum;

    private static int _numStarsLit;

    public static event Action OnLevelClear;

    private GameObject[] _innerGravityField;


    private void Start()
    {
        CreateStar.OnStarCreation += OnStarCreation;
        DestroyStar.OnStarDestruction += OnStarDestruction;
        _innerGravityField = GameObject.FindGameObjectsWithTag("|InnerGravityField|");
    }

    private void OnStarCreation()
    {
        Debug.Log("ClearLevel -- OnStarCreation");
        _numStarsLit++;
        if (_numStarsLit != totalStarNum) return;
        Debug.Log("equal");
        
        OnLevelClear?.Invoke();

        //TODO: implement me.
        //Delay until camera pans out of field
        //Invoke("DisableInnerFieldRender", 3);

        // The level clear screen needs to be delayed so that the camera has time to pan to the appropriate location,
        // and so that the player has enough time to see the constellation.
        Invoke(nameof(ShowClearImage), 9);
    }

    private static void OnStarDestruction()
    {
        _numStarsLit--;
    }

    private void ShowClearImage()
    {
        Debug.Log("ClearLevel -- ShowClearImage");

        clearLevelImage.SetActive(true);
        stardustCountText.SetActive(false);
        tutorialZoneText.SetActive(false);
    }
    
    private void DisableInnerFieldRender()
    {
        //Disable render for inner gravity field (or it will show when the camera pans out of the field)
        foreach (var field in _innerGravityField)
        {
            field.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
