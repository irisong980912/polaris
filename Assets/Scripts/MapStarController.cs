using System.Collections.Generic;
using UnityEngine;

public class MapStarController : MonoBehaviour
{
    private static bool _mapActive;
    private readonly HashSet<GameObject> _deactivatedStars = new HashSet<GameObject>();
    
    private void Start()
    {
        CameraSwitch.OnMapSwitch += SetMapActive;
    }

    private static void SetMapActive(bool mapActive)
    {
        _mapActive = mapActive;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_mapActive)
        {
            SetStarVisibility();
        }
        else
        {
            RevertStarVisibility();
        }
    }

    private void SetStarVisibility()
    {
        var litStarHashSet = new HashSet<GameObject>();
        //Disable all the stars, find all stars that are already created
        try
        {
            foreach (var o in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                var go = (GameObject) o;
                if (!go.tag.Contains("|Star|")) continue;
                if (go.GetComponent<Star>().isCreated)
                {
                    litStarHashSet.Add(go);
                }
                else
                {
                    _deactivatedStars.Add(go);
                }
            }
        }
        catch (UnityException)
        {
            print("No such tag");
        }
        
        //Remove all adjacent star of lit star from _deactivatedStars
        foreach (var litStar in litStarHashSet)
        {
            _deactivatedStars.Remove(litStar);
            switch (litStar.name)
            {
                case "Star 1":
                    _deactivatedStars.Remove(GameObject.Find("Star 2"));
                    break;
                case "Star 2":
                    _deactivatedStars.Remove(GameObject.Find("Star 1"));
                    _deactivatedStars.Remove(GameObject.Find("Star 3"));
                    break;
                case "Star 3":
                    _deactivatedStars.Remove(GameObject.Find("Star 2"));
                    _deactivatedStars.Remove(GameObject.Find("Star 4"));
                    _deactivatedStars.Remove(GameObject.Find("Star 5"));
                    break;
                case "Star 4":
                    _deactivatedStars.Remove(GameObject.Find("Star 3"));
                    _deactivatedStars.Remove(GameObject.Find("Star 5"));
                    _deactivatedStars.Remove(GameObject.Find("Star 7"));
                    break;
                case "Star 5":
                    _deactivatedStars.Remove(GameObject.Find("Star 3"));
                    _deactivatedStars.Remove(GameObject.Find("Star 4"));
                    _deactivatedStars.Remove(GameObject.Find("Star 6"));
                    break;
                case "Star 6":
                    _deactivatedStars.Remove(GameObject.Find("Star 5"));
                    _deactivatedStars.Remove(GameObject.Find("Star 7"));
                    break;
                case "Star 7":
                    _deactivatedStars.Remove(GameObject.Find("Star 4"));
                    _deactivatedStars.Remove(GameObject.Find("Star 6"));
                    break;
            }
        }

        //Disable all stars in _deactivatedStars
        foreach (var deactivatedStar in _deactivatedStars)
        {
            deactivatedStar.SetActive(false);
        }
    }

    private void RevertStarVisibility()
    {
        foreach (var deactivatedStar in _deactivatedStars)
        {
            deactivatedStar.SetActive(true);
        }
    }
}
