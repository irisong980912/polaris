using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public void PlayGame () 
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        print("You quit the game");
        Application.Quit();
    }
    
    private void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            PlayGame();
        }

    }
}