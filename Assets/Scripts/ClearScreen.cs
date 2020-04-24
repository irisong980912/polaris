using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ClearScreen : MonoBehaviour
{
    public GameObject clearLevelMenu;
    public string sceneName;

    public PlayerInputActions _inputAction;

    void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();

    }

    public void ChangeGameScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        //transform.parent.gameObject.SetActive(false);
        clearLevelMenu.SetActive(false);
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("StartMenu");
        Debug.Log("Loading menu...");
        clearLevelMenu.SetActive(false);

    }

    public void ReplayLevel()
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentLevel);

        clearLevelMenu.SetActive(false);
    }

    //InputActions
    //Activates all actions in Player action maps (action maps are Player and UI)
    private void OnEnable()
    {
        _inputAction.UI.Enable();
        //_inputAction.Player.Disable();

    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        //_inputAction.UI.Disable();
        _inputAction.Player.Enable();

    }
}
