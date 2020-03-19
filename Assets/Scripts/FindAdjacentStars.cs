using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

/// <summary>
/// attach to a canvas object 
/// 1. determine which stars to point to
/// 2. have each star owning the canvas and the pointer image
/// 3. make the image follow the star locations (lookat)
/// 4. make those image the children of the canvas
/// 5. make the cursor stay within the field of view
///
/// 6. when selecting the cursor, show a live image of the star
///  within what diameter of distance?
/// need to build a tree and then breath first search the closest stars ????
/// simply, the 2 most adjacent stars
/// </summary>
public class FindAdjacentStars : MonoBehaviour
{
    
    public Transform currStar;

    private List<GameObject> AdjacentStarList;

    private void Start()
    {
        
        // TODO: need to listen to the event of isometric cam. 
        IsometricStarView.OnInitiatePointerToAdjacentStars += OnInitiatePointerToAdjacentStars;
    }
    
    private void OnInitiatePointerToAdjacentStars(Transform star)
    {
        // TODO: find the star that player is in 
        currStar = star;
        FindTwoAdjacentStars();
        PopulateChildList();
    }

    private void PopulateChildList()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "StarButton1")
            {
                print("StarButton1");
                child.GetComponent<StarIconManager>().starToGo = AdjacentStarList[0].transform;
            } else if (child.name == "StarButton2")
            {
                print("StarButton2");
                child.GetComponent<StarIconManager>().starToGo = AdjacentStarList[1].transform;
            }
        }
    }

    private void FindTwoAdjacentStars()
    {
        print("FindTwoAdjacentStars");
        AdjacentStarList = new List<GameObject>();
        var empty = new GameObject();
        AdjacentStarList.Add(empty);
        AdjacentStarList.Add(empty);
        var minDis1 = double.PositiveInfinity;;
        var minDis2 = double.PositiveInfinity;;
        // select the two adjacent stars
        try
        {
            foreach (var o in FindObjectsOfType(typeof(GameObject)))
            {
                var go = (GameObject) o;
                if (!go.tag.Contains("|Star|")) continue;
                if (go.name == currStar.name) continue;
                var distanceToStar = Vector3.Distance(go.transform.position, currStar.position);
                if (!(distanceToStar < Math.Max(minDis1, minDis2))) continue;
                if (distanceToStar < minDis1)
                {
                    minDis2 = minDis1;
                    AdjacentStarList[1] = AdjacentStarList[0];
                        
                    minDis1 = distanceToStar;
                    AdjacentStarList[0] = go;

                }
                else
                {
                    minDis2 = distanceToStar;
                    AdjacentStarList[1] = go;
                }
            }
        } catch (UnityException)
        {
            print("No such tag");
        }
        
        print(AdjacentStarList[0].name);
        print(AdjacentStarList[1].name);
    
    }
}