using System;
using TMPro;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class StarIconManager : MonoBehaviour
{
    
    private Image _buttonImg;
    
    public Sprite litStarImage;
    public Sprite litStarImageSelected;
    public Sprite darkStarImage;
    public Sprite darkStarImageSelected;

    public bool _isLit;

    public Transform starToGo;
    
    public TextMeshProUGUI starBtnText;

    public Transform player;
    public static event Action<Transform> OnHoverStart;
    
    public static event Action<Transform> OnHoverStop;
    
    public static event Action<Transform> OnSelectStar;
    
    private void Start()
    {
        _buttonImg = GetComponent<Image>();
        
        if (starToGo.GetComponent<Star>().isCreated)
        {
            _buttonImg.sprite = litStarImage; 
        }
        else
        {
            _buttonImg.sprite = darkStarImage; 
        }
        
        
    }
    
    // OnPointerEnter
    public void ShowBeamDir () 
    {
        if (!starToGo) return;
        
        if (starToGo.GetComponent<Star>().isCreated)
        {
            _buttonImg.sprite = litStarImageSelected; 
        }
        else
        {
            _buttonImg.sprite = darkStarImageSelected; 
        }
        
        Debug.Log("mouse hover enter");
        OnHoverStart?.Invoke(starToGo);
        
    }
    
    // OnPointerExit
    public void HideBeamDir () 
    {
        if (!starToGo) return;
        
        if (starToGo.GetComponent<Star>().isCreated)
        {
            _buttonImg.sprite = litStarImage; 
        }
        else
        {
            _buttonImg.sprite = darkStarImage; 
        }
        
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