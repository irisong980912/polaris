using System;
using UnityEngine;
using System.Collections.Generic;

public class ClearLevel : MonoBehaviour
{
    [SerializeField] private GameObject clearLevelImage;

    public int totalStarNum;

    private static int _numStarsLit;

    public static event Action OnLevelClear;

    private GameObject[] innerGravityfield;


    private void Start()
    {
        CreateStar.OnStarCreation += OnStarCreation;
        DestroyStar.OnStarDestruction += OnStarDestruction;
        innerGravityfield = GameObject.FindGameObjectsWithTag("|InnerGravityField|");
    }

    private void OnStarCreation()
    {
        Debug.Log("ClearLevel -- OnStarCreation");
        _numStarsLit++;
        if (_numStarsLit != totalStarNum) return;
        Debug.Log("equal");

        // call camera and animateBean
        OnLevelClear?.Invoke();


        //Delay until camera pans out of field
        //Invoke("DisableInnerFieldRender", 3);

        // The level clear screen needs to be delayed so that the camera has time to pan to the appropriate location,
        // and so that the player has enough time to see the constellation.
        Invoke(nameof(ShowClearImage), 10);
    }

    private static void OnStarDestruction()
    {
        _numStarsLit--;
    }

    private void ShowClearImage()
    {
        Debug.Log("ClearLevel -- ShowClearImage");

        clearLevelImage.SetActive(true);
    }
    
    private void DisableInnerFieldRender()
    {
        //Disable render for inner gravity field (or it will show when the camera pans out of the field)
        foreach (GameObject field in innerGravityfield)
        {
            field.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
