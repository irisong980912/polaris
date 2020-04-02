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
///
///  TODO: change adjacent 2 stars to stars within radius
public class FindAdjacentStars : MonoBehaviour
{
    
    // within 2000 units. At most 4 buttons 
    // within 180 units, no, self.
    
    public Transform curStarGravityCore;

    private List<GameObject> adjacentStarList;

    public int adjacentStarNum;

    public float gravityRadius = 180.0f;

    public float adjacentDis = 2000.0f;

    private void Start()
    {
        
        // TODO: need to listen to the event of isometric cam. 
        IsometricStarPosManager.OnIsometricStarView += OnIsometricStarView;
    }
    
    // start find adjacent stars when enter the star gravity field
    private void OnIsometricStarView(bool isIso, Transform starGravityCore)
    {
        // TODO: find the star that player is in 
        curStarGravityCore = starGravityCore;
        PopulateAdjacentStarList();
        PopulateStarBtns();
    }

    private void PopulateStarBtns()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "StarButton1")
            {
                if (adjacentStarNum < 1) return;
                child.GetComponent<StarSelectButtonManager>().starToGo = adjacentStarList[0].transform;
            } else if (child.name == "StarButton2")
            {
                if (adjacentStarNum < 2) return;
                child.GetComponent<StarSelectButtonManager>().starToGo = adjacentStarList[1].transform;
            } else if (child.name == "StarButton3")
            {
                if (adjacentStarNum < 3) return;
                child.GetComponent<StarSelectButtonManager>().starToGo = adjacentStarList[2].transform;
            } else if (child.name == "StarButton4")
            {
                if (adjacentStarNum < 4) return;
                child.GetComponent<StarSelectButtonManager>().starToGo = adjacentStarList[3].transform;
            }
        }
    }

    private void PopulateAdjacentStarList()
    {
        
        print("FindTwoAdjacentStars");
        adjacentStarList = new List<GameObject>();
        var empty = new GameObject();
        adjacentStarList.Add(empty);
        adjacentStarList.Add(empty);
        adjacentStarList.Add(empty);
        adjacentStarList.Add(empty);

        try
        {
            foreach (var o in FindObjectsOfType(typeof(GameObject)))
            {
                if (adjacentStarNum >= 4) break;
                var go = (GameObject) o;
                if (!go.tag.Contains("|Star|")) continue;
                var distanceToStar = Vector3.Distance(go.transform.position, curStarGravityCore.position);
                // self
                if (distanceToStar <= gravityRadius) continue;
                if (distanceToStar > adjacentDis) continue;
                
                adjacentStarList[adjacentStarNum] = go;
                adjacentStarNum++;
            }
        } catch (UnityException)
        {
            print("No such tag");
        }
        
        
        
        // var empty = new GameObject();
        // adjacentStarList.Add(empty);
        // adjacentStarList.Add(empty);
        // var minDis1 = double.PositiveInfinity;;
        // var minDis2 = double.PositiveInfinity;;
        // // select the two adjacent stars
        // try
        // {
        //     foreach (var o in FindObjectsOfType(typeof(GameObject)))
        //     {
        //         var go = (GameObject) o;
        //         if (!go.tag.Contains("|Star|")) continue;
        //         if (go.name == currStar.name) continue;
        //         var distanceToStar = Vector3.Distance(go.transform.position, currStar.position);
        //         if (!(distanceToStar < Math.Max(minDis1, minDis2))) continue;
        //         if (distanceToStar < minDis1)
        //         {
        //             minDis2 = minDis1;
        //             adjacentStarList[1] = adjacentStarList[0];
        //                 
        //             minDis1 = distanceToStar;
        //             adjacentStarList[0] = go;
        //
        //         }
        //         else
        //         {
        //             minDis2 = distanceToStar;
        //             adjacentStarList[1] = go;
        //         }
        //     }
        // } catch (UnityException)
        // {
        //     print("No such tag");
        // }

    }

    private void OnDisable()
    {
        //Prevent event from looking for prescribed object that is removed on Reload of scene, by unsubscribing.
        IsometricStarPosManager.OnIsometricStarView -= OnIsometricStarView;
    }
}