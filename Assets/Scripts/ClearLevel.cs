using System;


using UnityEngine;
using UnityEngine.UI;
public class ClearLevel : MonoBehaviour
{
    [SerializeField] private GameObject clearLevelImage;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("|Player|"))
        {
            clearLevelImage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("|Player|"))
        //{
        //    clearLevelImage.SetActive(false);
        //}
    }

}
