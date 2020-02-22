using UnityEngine;

public class ClearLevel : MonoBehaviour
{
    [SerializeField] private GameObject clearLevelImage;

    public int totalStarNum;

    private static int _starsLit;

    private void Start()
    {
        CreateStar.OnStarCreation += OnStarCreation;
        DestroyStar.OnStarDestruction += OnStarDestruction;
    }

    private void OnStarCreation()
    {
        _starsLit++;
        if (_starsLit == totalStarNum)
        {
            ShowClearImage();
        }
    }

    private static void OnStarDestruction()
    {
        _starsLit--;
    }

    private void ShowClearImage()
    {
        clearLevelImage.SetActive(true);
    }
    
}
