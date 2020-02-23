﻿using System;
using UnityEngine;

public class ClearLevel : MonoBehaviour
{
    [SerializeField] private GameObject clearLevelImage;

    public int totalStarNum;

    private static int _numStarsLit;

    public static event Action OnLevelClear;

    private void Start()
    {
        CreateStar.OnStarCreation += OnStarCreation;
        DestroyStar.OnStarDestruction += OnStarDestruction;
    }

    private void OnStarCreation()
    {
        _numStarsLit++;
        if (_numStarsLit != totalStarNum) return;
        OnLevelClear?.Invoke();
        // The level clear screen needs to be delayed so that the camera has time to pan to the appropriate location,
        // and so that the player has enough time to see the constellation.
        Invoke(nameof(ShowClearImage), 7);
    }

    private static void OnStarDestruction()
    {
        _numStarsLit--;
    }

    private void ShowClearImage()
    {
        clearLevelImage.SetActive(true);
    }
    
}
