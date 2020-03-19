using System;
using TMPro;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class StarIconManager : MonoBehaviour
{

    public Transform starToGo;
    
    public TextMeshProUGUI starBtnText;

    public Transform player;
    public static event Action<Transform> OnHoverStart;
    
    public static event Action<Transform> OnHoverStop;
    
    public static event Action<Transform> OnSelectStar;
    
    // show the image of the star when hovering
    public void ShowBeamDir () 
    {
        Debug.Log("mouse hover enter");
        OnHoverStart?.Invoke(starToGo);
        
    }
    
    public void HideBeamDir () 
    {
        Debug.Log("mouse hover exit");
        OnHoverStop?.Invoke(starToGo);
        
    }
    
    public void SelectStarToSlingShot()
    {
        print("!!!!!!!!!!!!!mouse onclick -- SelectStarToSlingShot");
        if (!starToGo) return;
        OnSelectStar?.Invoke(starToGo);
    }
    
    private void Update()
    {
        if (!player) return;
        if (!starToGo) return;
        var disToStar = Vector3.Distance(player.position, starToGo.position);
        disToStar = (float) Math.Round(disToStar, 0);
        starBtnText.text = disToStar + "m";
    }

    
}