using System;
using UnityEngine;

public class StardustPickup : MonoBehaviour
{
    public int stardustValue = 1;
    // TODO: implement me.
    //public AudioClip clip;
    public GameObject stardust;

    public void OnTriggerStay(Collider other)
    {
        var player = other.GetComponent<ThirdPersonPlayer>();
        if (player is null) return;

        if (Vector3.Distance(transform.position, other.transform.position) > 5)
        {
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, 2);
        }
        else
        {
            Debug.Log("pickup add");
            player.stardust += stardustValue;
            player.inventory.Add(stardust);
            gameObject.SetActive(false);
        }
        
    }

}
