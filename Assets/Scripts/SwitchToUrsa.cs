
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SwitchToUrsa : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame () 
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {
        if (Input.GetButton("Jump"))
        {
            PlayGame();
        }
    }
}
