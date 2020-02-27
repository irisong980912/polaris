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
    // public AudioSource bgmLayer8;
    // public AudioSource bgmLayer9;
    // public AudioSource bgmLayer10;
    // public AudioSource bgmLayer11;
    // public AudioSource bgmLayer12;
    // public AudioSource bgmLayer13;
    // public AudioSource bgmLayer14;

    private List<AudioSource> _allLayers;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        _allLayers = new List<AudioSource>
        {
            bgmLayer1.GetComponent<AudioSource>(),
            bgmLayer2.GetComponent<AudioSource>(),
            bgmLayer3.GetComponent<AudioSource>(),
            bgmLayer4.GetComponent<AudioSource>(),
            bgmLayer5.GetComponent<AudioSource>(),
            bgmLayer6.GetComponent<AudioSource>(),
            bgmLayer7.GetComponent<AudioSource>(),
            // bgmLayer8.GetComponent<AudioSource>(),
            // bgmLayer9.GetComponent<AudioSource>(),
            // bgmLayer10.GetComponent<AudioSource>(),
            // bgmLayer11.GetComponent<AudioSource>(),
            // bgmLayer12.GetComponent<AudioSource>(),
            // bgmLayer13.GetComponent<AudioSource>(),
            // bgmLayer14.GetComponent<AudioSource>()
        };


        foreach (var layer in _allLayers)
        {
            layer.volume = 0;
        }

        _allLayers[0].volume = 1;
        
        CreateStar.OnStarCreation += OnStarCreation;
        DestroyStar.OnStarDestruction += OnStarDestruction;
    }

    private void OnStarCreation()
    {
        foreach (var layer in _allLayers.Where(layer => !(layer.volume > 0.1)))
        {
            layer.volume = 1;
            break;
        }
    }

    private void OnStarDestruction()
    {
        var previous = _allLayers[0];
        foreach (var layer in _allLayers)
        {
            if (!(layer.volume > 0.1))
            {
                previous.volume = 0;
            }
            previous = layer;
        }
        
    }
    
}
