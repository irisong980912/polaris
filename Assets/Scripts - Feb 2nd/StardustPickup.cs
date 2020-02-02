using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StardustPickup : MonoBehaviour
{
    public int stardustValue = 1;
    //public AudioClip clip;
    public GameObject stardust;

    public void OnTriggerEnter(Collider collision)
    {
        ThirdPersonPlayer player = collision.GetComponent<ThirdPersonPlayer>();

        if(player != null)
        {
            player.stardust += stardustValue;
            player.inventory.Add(stardust);
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
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
