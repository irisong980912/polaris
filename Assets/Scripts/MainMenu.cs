using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class MainMenu : MonoBehaviour
{
    public EventSystem ES;
    private GameObject _storeSelected;
    PlayerInputActions _inputAction;
    [FormerlySerializedAs("Interact")] public InputAction interact;

    public void PlayGame () 
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        print("You quit the game");
        Application.Quit();
    }
    
    void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();
        interact = _inputAction.Player.Interact;

    }
    
    //InputActions
    //Activates all actions in Player action maps (action maps are Player and UI)
    private void OnEnable()
    {
        interact.Enable();
        _inputAction.Player.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        interact.Disable();
        _inputAction.Player.Disable();
    }
    
    private void Update()
    {
        // if (ES.currentSelectedGameObject != StoreSelected)
        // {
        //     if (ES.currentSelectedGameObject == null)
        //     {
        //         ES.SetSelectedGameObject(StoreSelected);
        //     }
        //     else
        //     {
        //         StoreSelected = ES.currentSelectedGameObject;
        //     }
        // }
        /*
        if (Input.GetButton("Fire2"))
        {
            PlayGame();
        }
        */
        if (interact.triggered)
        {
            PlayGame();
        }
    }
}