using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  TODO: when scattering the stardustm implement the transition animation that stardust
///  is being distributed from the core
/// </summary>
public class StardustPickup : MonoBehaviour
{
    public int stardustValue = 1;
    //public AudioClip clip;
    public GameObject stardust;

    public void OnTriggerEnter(Collider collision)
    {
        

        ThirdPersonPlayer player = collision.GetComponent<ThirdPersonPlayer>();
        Debug.Log("pickup");
        

        if (player != null)
        {
            Debug.Log("pickup add");
            player.stardust += stardustValue;
            player.inventory.Add(stardust);
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

}
