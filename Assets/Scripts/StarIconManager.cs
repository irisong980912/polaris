using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StarIconManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Sprite litStarImage;
    public Sprite litStarImageSelected;
    public Sprite darkStarImage;
    public Sprite darkStarImageSelected;
   
    public TextMeshProUGUI starBtnText;

    public Transform player;
    public Transform starToGo;
    public static event Action<Transform> OnHoverStart;
    public static event Action<Transform> OnHoverStop;
    public static event Action<Transform> OnSelectStar;
    //Event systems
    public EventSystem es;
    public GameObject defaultButtonSelected;
    
    private Image _buttonImg;
    private GameObject _storeSelected;

    private PlayerInputActions _inputAction;

    private void Awake()
    {
        _inputAction = new PlayerInputActions();
    }

    private void Start()
    {
        es.SetSelectedGameObject(defaultButtonSelected);
        _buttonImg = GetComponent<Image>();
        
        _buttonImg.sprite = starToGo.GetComponent<Star>().isCreated ? litStarImage : darkStarImage;
    }
    
    // OnPointerEnter
    public void ShowBeamDir () 
    {
        if (!starToGo) return;
        
        _buttonImg.sprite = starToGo.GetComponent<Star>().isCreated ? litStarImageSelected : darkStarImageSelected;
        
        OnHoverStart?.Invoke(starToGo);
    }
    
    // OnPointerExit
    public void HideBeamDir () 
    {
        if (!starToGo) return;
        
        _buttonImg.sprite = starToGo.GetComponent<Star>().isCreated ? litStarImage : darkStarImage;
        
        OnHoverStop?.Invoke(starToGo);
    }
    
    public void SelectStarToSlingShot()
    {
        if (!starToGo) return;
        OnSelectStar?.Invoke(starToGo);
    }

    //for controller selection, add star image switches
    public void OnSelect(BaseEventData eventData) { }
    public void OnDeselect(BaseEventData eventData) { }

    private void Update()
    {
        if (!player) return;
        if (!starToGo) return;
        var disToStar = Vector3.Distance(player.position, starToGo.position);
        disToStar = (float) Math.Round(disToStar, 0);
        starBtnText.text = disToStar + "m";

        if (es.currentSelectedGameObject == _storeSelected) return;
        if (es.currentSelectedGameObject is null)
        {
            es.SetSelectedGameObject(_storeSelected);
        }
        else
        {
            _storeSelected = es.currentSelectedGameObject;
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
