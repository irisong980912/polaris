using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public AsteroidCutSceneController controller;   
    public void OnTriggerExit(Collider collision)
    {
        try
        {
            if (collision.gameObject.tag.Contains("|Player|"))
            {
                //Play the cutscene
                controller.Play();

                //Return player to desired location
                collision.gameObject.transform.position = new Vector3 (-6.218f, 1.5874f, 8.034f);
            }
        }
        catch (UnityException)
        {
            print("No such tag");
        }

        Debug.Log("exit bounds");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
