using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInnerGravityField : MonoBehaviour
{
    MeshRenderer renderInside;


    private void Start()
    {
        renderInside = gameObject.GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("MainCamera"))
        {
            renderInside.enabled = true;
            Debug.Log("enabled");
           
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.CompareTag("MainCamera"))
        {
            renderInside.enabled = false;
            Debug.Log("disabled");

        }
    }
}
