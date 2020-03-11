
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonImageSwitch : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    
    private Image _buttonImg;
    public Sprite originalButtonImage;
    public Sprite newButtonImage;


    private void Start()
    {
        _buttonImg = GetComponent<Image>();
        
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _buttonImg.sprite = newButtonImage; 
        Debug.Log("Mouse Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _buttonImg.sprite = originalButtonImage; 
        Debug.Log("Mouse Exit");
    }
}