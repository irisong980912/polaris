using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class StarIconManager : MonoBehaviour, ISelectHandler, IDeselectHandler
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

    //Event systems
    public EventSystem ES;
    public GameObject defaultbuttonSelected;
    private GameObject _storeSelected;


    PlayerInputActions _inputAction;

    void Awake()
    {
        _inputAction = new PlayerInputActions();
    }

    private void Start()
    {
        ES.SetSelectedGameObject(defaultbuttonSelected);
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

    //for controller selection, add star image switches
    public void OnSelect(BaseEventData eventData)
    {
        
    }

    public void OnDeselect(BaseEventData eventData)
    {

    }



    private void Update()
    {
        if (!player) return;
        if (!starToGo) return;
        var disToStar = Vector3.Distance(player.position, starToGo.position);
        disToStar = (float) Math.Round(disToStar, 0);
        starBtnText.text = disToStar + "m";

        if (ES.currentSelectedGameObject != _storeSelected)
        {
            if (ES.currentSelectedGameObject == null)
            {
                ES.SetSelectedGameObject(_storeSelected);
            }
            else
            {
                _storeSelected = ES.currentSelectedGameObject;
            }
        }
    }

    //InputActions
    //Activates all actions in Player action maps (action maps are Player and UI)
    private void OnEnable()
    {
        _inputAction.UI.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        _inputAction.UI.Disable();
    }
}