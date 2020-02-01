using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StardustPickup : MonoBehaviour
{
    public int stardustValue = 1;
    //public AudioClip clip;
    //public GameItem item;

    public void OnTriggerEnter(Collider collision)
    {
        ThrirdPersonPlayer player = collision.GetComponent<ThrirdPersonPlayer>();

        if(player != null)
        {
            player.stardust += stardustValue;
            Destroy(gameObject);
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
