using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class MainMenu : MonoBehaviour
{
    public EventSystem ES;
    private GameObject _storeSelected;
    private PlayerInputActions _inputAction;
    public InputAction interact;

    public void PlayGame () 
    {
        Debug.Log("start game");
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
        Debug.Log("start gamesss");
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
        
        /*
        if (Input.GetButton("Fire2"))
        {
            PlayGame();
        }
        */
        /*
        if (interact.triggered)
        {
            PlayGame();
        }
        */
    }
}