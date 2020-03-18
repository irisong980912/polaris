using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject starCountDisplay;

    public EventSystem ES;
    private GameObject _storeSelected;

    PlayerInputActions _inputAction;

    public InputAction menuAction;
    public InputAction interact;

    void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();

        menuAction = _inputAction.Player.Menu;
        interact = _inputAction.Player.Interact;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (menuAction.triggered)
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

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

    public void Resume()
    {
        pauseMenu.SetActive(false);
        starCountDisplay.SetActive(true);
        Time.timeScale = 1f;

        gameIsPaused = false;
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        starCountDisplay.SetActive(false);
        Time.timeScale = 0f;

        gameIsPaused = true;

    }

    public void LoadMenu()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene("StartMenu");
        Debug.Log("Loading menu...");
    }

    public void OpenSettings()
    {
        Debug.Log("Opening settings...");
    }

    //InputActions
    //Activates all actions in Player action maps (action maps are Player and UI)
    private void OnEnable()
    {
        menuAction.Enable();
        interact.Enable();
        _inputAction.Player.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        menuAction.Disable();
        interact.Disable();
        _inputAction.Player.Disable();
    }
}
