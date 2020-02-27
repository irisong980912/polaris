using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BgmLayers : MonoBehaviour
{
    public AudioSource bgmLayer1;
    public AudioSource bgmLayer2;
    public AudioSource bgmLayer3;
    public AudioSource bgmLayer4;
    public AudioSource bgmLayer5;
    public AudioSource bgmLayer6;
    public AudioSource bgmLayer7;
    public AudioSource bgmLayer8;
    public AudioSource bgmLayer9;
    public AudioSource bgmLayer10;
    public AudioSource bgmLayer11;
    public AudioSource bgmLayer12;
    public AudioSource bgmLayer13;
    public AudioSource bgmLayer14;

    private List<AudioSource> _allLayers;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        _allLayers = new List<AudioSource>
        {
            bgmLayer1,
            bgmLayer2,
            bgmLayer3,
            bgmLayer4,
            bgmLayer5,
            bgmLayer6,
            bgmLayer7,
            bgmLayer8,
            bgmLayer9,
            bgmLayer10,
            bgmLayer11,
            bgmLayer12,
            bgmLayer13,
            bgmLayer14
        };


        foreach (var layer in _allLayers)
        {
            layer.enabled = false;
        }

        _allLayers[0].enabled = true;
        
        CreateStar.OnStarCreation += OnStarCreation;
        DestroyStar.OnStarDestruction += OnStarDestruction;
    }

    private void OnStarCreation()
    {
        foreach (var layer in _allLayers.Where(layer => !layer.enabled))
        {
            layer.enabled = true;
            break;
        }
    }

    private void OnStarDestruction()
    {
        var previous = _allLayers[0];
        foreach (var layer in _allLayers)
        {
            if (!layer.enabled)
            {
                previous.enabled = false;
            }
            previous = layer;
        }
        
    }
    
}
