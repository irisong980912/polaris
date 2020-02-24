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
        if (player.CompareTag("|Player|"))
        {
            renderInside.enabled = true;
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.CompareTag("|Player|"))
        {
            renderInside.enabled = false;
        }
    }
}
