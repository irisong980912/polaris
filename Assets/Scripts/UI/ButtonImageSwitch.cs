
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonImageSwitch : MonoBehaviour, ISelectHandler, IDeselectHandler //, IPointerExitHandler, IPointerEnterHandler
{
    
    private Image _buttonImg;
    public Sprite originalButtonImage;
    public Sprite newButtonImage;

 
    private void Start()
    {
        _buttonImg = GetComponent<Image>();
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        _buttonImg.sprite = newButtonImage;
        Debug.Log("selected");
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _buttonImg.sprite = originalButtonImage;
        Debug.Log("deselect");
    }
}