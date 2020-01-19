using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGem : MonoBehaviour
{
    public GameObject star;
    private bool _canInteract;
    
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown("e") && _canInteract) //&& _canPickUp
        {
            DestroyItem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("GravityObject")) return;
        _canInteract = true;
        print("ok");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("GravityObject")) return;
        _canInteract = false;
        print("out");
    }

    private void DestroyItem()
    {
        Destroy(star);
    }
}
