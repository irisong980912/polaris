using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class ClearLevel : MonoBehaviour
{
    public GameObject clearLevelMenu;
    public GameObject stardustCountText;
    public GameObject tutorialZoneText;

    public EventSystem ES;
    private GameObject _storeSelected;
    public GameObject defaultButtonSelected;

    public int totalStarNum;

    //private static int _numStarsLit;
    public int _numStarsLit;

    public static event Action OnLevelClear;

    private GameObject[] _innerGravityField;


    PlayerInputActions _inputAction;


    void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();

    }
    private void Start()
    {
        _numStarsLit = 0;
        CreateStar.OnStarCreation += OnStarCreation;
        DestroyStar.OnStarDestruction += OnStarDestruction;
        _innerGravityField = GameObject.FindGameObjectsWithTag("|InnerGravityField|");
    }

    private void OnStarCreation()
    {
        Debug.Log("ClearLevel -- OnStarCreation");
        _numStarsLit++;
        if (_numStarsLit != totalStarNum) return;
        Debug.Log("equal");
        
        OnLevelClear?.Invoke();

        //TODO: implement me.
        //Delay until camera pans out of field
        //Invoke("DisableInnerFieldRender", 3);

        // The level clear screen needs to be delayed so that the camera has time to pan to the appropriate location,
        // and so that the player has enough time to see the constellation.
        Invoke(nameof(ShowClearMenu), 9);
    }
    /*
    private static void OnStarDestruction()
    {
        _numStarsLit--;
    }
    */
    private void OnStarDestruction()
    {
        if (_numStarsLit > 0)
        {
            _numStarsLit--;
        }
    }
    private void ShowClearMenu()
    {
        Debug.Log("ClearLevel -- ShowClearMenu");

        clearLevelMenu.SetActive(true);
        stardustCountText.SetActive(false);
        tutorialZoneText.SetActive(false);

        ES.SetSelectedGameObject(defaultButtonSelected);
    }

    

    private void DisableInnerFieldRender()
    {
        //Disable render for inner gravity field (or it will show when the camera pans out of the field)
        foreach (var field in _innerGravityField)
        {
            field.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void Update()
    {
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

        //Prevent event from looking for prescribed object that is removed on Reload of scene, by unsubscribing.
        CreateStar.OnStarCreation -= OnStarCreation;
        DestroyStar.OnStarDestruction -= OnStarDestruction;
    }
}
