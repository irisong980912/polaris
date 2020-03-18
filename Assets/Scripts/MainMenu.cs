using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public EventSystem ES;
    private GameObject _storeSelected;
    public GameObject mainUI;

    //Button Menus
    public GameObject settingsMenu;
    public GameObject levelMenu;

    //Check submenu status
    bool settingsOpen;
    bool levelOpen;

    //Input Actions
    private PlayerInputActions _inputAction;
    public InputAction cancel;

    //Access Buttons
    public GameObject startButton;
    public GameObject levelButton;
    public GameObject settingsButton;
    public GameObject quitButton;

    Vector2 startLocation;
    Vector2 endLocation;

    void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();
        cancel = _inputAction.UI.Cancel;
    }

    private void Start()
    {
        endLocation = new Vector2(mainUI.transform.position.x, mainUI.transform.position.y + 8);
    }

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
    
    public void OpenLevelSelection()
    {
        if (!levelOpen)
        {
            print("Opened level selector");
            levelMenu.SetActive(true);
            levelOpen = true;

            startButton.GetComponent<Button>().interactable = false;
            //levelButton.GetComponent<Button>().interactable = false;
            settingsButton.GetComponent<Button>().interactable = false;
            quitButton.GetComponent<Button>().interactable = false;
        } else
        {
            OnCancel();
        }
    }

    public void OpenSettings()
    {
        if (!settingsOpen)
        {
            print("Settings opened");
            //mainUI.transform.position = Vector2.Lerp(mainUI.transform.position, endLocation, 2 * Time.deltaTime);

            startLocation = new Vector2(mainUI.transform.position.x, mainUI.transform.position.y - 8);

            StartCoroutine(DelaySettings());
            settingsOpen = true;

            startButton.GetComponent<Button>().interactable = false;
            levelButton.GetComponent<Button>().interactable = false;
            //settingsButton.GetComponent<Button>().interactable = false;
            quitButton.GetComponent<Button>().interactable = false;
        } else
        {
            OnCancel();
        }

    }

    //Returns Main Menu to initial state / closes Button Menus
    void OnCancel(){

        if (settingsOpen)
        {
            settingsMenu.SetActive(false);

            //mainUI.transform.position = Vector2.Lerp(mainUI.transform.position, startLocation, 2 * Time.deltaTime);
            settingsOpen = false;

            startButton.GetComponent<Button>().interactable = true;
            levelButton.GetComponent<Button>().interactable = true;
            //settingsButton.GetComponent<Button>().interactable = true;
            quitButton.GetComponent<Button>().interactable = true;
        }

        if (levelOpen)
        {
            levelMenu.SetActive(false);
            levelOpen = false;

            startButton.GetComponent<Button>().interactable = true;
            //levelButton.GetComponent<Button>().interactable = true;
            settingsButton.GetComponent<Button>().interactable = true;
            quitButton.GetComponent<Button>().interactable = true;
        }
       
    }


    private void Update()
    {
        //Prevent bug where using the clicking the mouse breaks controller input
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

    IEnumerator DelaySettings()
    {  
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1 * Time.deltaTime);

        settingsMenu.SetActive(true);
    }

    private void OnEnable()
    {
        Debug.Log("start gamesss");
        cancel.Enable();
        _inputAction.Player.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        cancel.Disable();
        _inputAction.Player.Disable();
    }
}