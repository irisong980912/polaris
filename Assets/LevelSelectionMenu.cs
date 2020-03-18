using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelSelectionMenu : MonoBehaviour
{
    public Dropdown dropDown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public void OnLevelSelect()
    {
        if (dropDown.value == 1)
        {
            SceneManager.LoadScene("Ursa Minor");
            Debug.Log("Loading Ursa Minor...");
        }

        if(dropDown.value == 2)
        {
            SceneManager.LoadScene("Cassiopeia");
            Debug.Log("Loading Cassiopeia...");
        }
    }
}
