using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGem : MonoBehaviour
{
    public GameObject star;
    private bool _canPickUp;
    
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown("e")) //&& _canPickUp
        {
            DestroyItem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        _canPickUp = true;
        print("ok");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        _canPickUp = false;
        print("out");
    }

    private void DestroyItem()
    {
        Destroy(star);
    }
}
