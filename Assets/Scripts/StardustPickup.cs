using UnityEngine;

public class StardustPickup : MonoBehaviour
{
    public int stardustValue = 1;
    // TODO: implement me.
    //public AudioClip clip;
    public GameObject stardust;

    public void OnTriggerEnter(Collider collision)
    {
        var player = collision.GetComponent<ThirdPersonPlayer>();
        Debug.Log("pickup");

        if (player is null) return;
        Debug.Log("pickup add");
        player.stardust += stardustValue;
        player.inventory.Add(stardust);
        gameObject.SetActive(false);

    }

}
