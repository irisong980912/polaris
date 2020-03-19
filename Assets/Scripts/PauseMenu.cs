using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject starCountDisplay;
    public GameObject tutorialDisplay;
    public GameObject clearMenu;

    public EventSystem ES;
    private GameObject _storeSelected;

    //Access Buttons
    public GameObject resumeButton;
    public GameObject settingsButton;
    public GameObject menuButton;

    //Check submenu status
    bool settingsOpen;

    PlayerInputActions _inputAction;

    public InputAction menuAction;
    public InputAction cancel;

    void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();

        menuAction = _inputAction.Player.Menu;
        cancel = _inputAction.UI.Cancel;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (menuAction.triggered && clearMenu.activeSelf == false)
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

        if (cancel.triggered)
        {
            OnCancel();

            //controller navigation breaks if you dont set a new default selected gameobject
            ES.SetSelectedGameObject(settingsButton);

        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        starCountDisplay.SetActive(true);
        tutorialDisplay.SetActive(true);

        gameIsPaused = false;
    }

    void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        starCountDisplay.SetActive(false);
        tutorialDisplay.SetActive(false);

        ES.SetSelectedGameObject(resumeButton);
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
        if (!settingsOpen)
        {
            Time.timeScale = 0f;
            print("Settings opened");
            settingsMenu.SetActive(true);
            settingsOpen = true;

            resumeButton.GetComponent<Button>().interactable = false;
            //settingsButton.GetComponent<Button>().interactable = false;
            menuButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            OnCancel();
        }
    }

    void OnCancel()
    {

        if (settingsOpen)
        {
            settingsMenu.SetActive(false);

            settingsOpen = false;

            resumeButton.GetComponent<Button>().interactable = true;
            //settingsButton.GetComponent<Button>().interactable = true;
            menuButton.GetComponent<Button>().interactable = true;
        }

    }
    //InputActions
    //Activates all actions in Player action maps (action maps are Player and UI)
    private void OnEnable()
    {
        menuAction.Enable();
        cancel.Enable();
        _inputAction.UI.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        menuAction.Disable();
        cancel.Disable();
        _inputAction.UI.Disable();
    }
}
