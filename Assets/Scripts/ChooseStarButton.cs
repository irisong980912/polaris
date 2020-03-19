using System;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class ChooseStarButton : MonoBehaviour
{

    public Transform starToGo;
    
    public static event Action<Transform> OnSelectStar;
    
    // show the image of the star when hovering
    public void ShowFrontalImage () 
    {
        Debug.Log("mouse hover");
        
    }

    public void SelectStarToSlingShot()
    {
        print("mouse onclick -- SelectStarToSlingShot");

        starToGo = GetComponent<PointerToStar>().StarToPoint.transform;
        
        OnSelectStar?.Invoke(starToGo);
    }

    
}