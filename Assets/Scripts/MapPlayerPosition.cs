using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlayerPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }
    
    //Update mapPlayerPosition with player position
    private void FollowPlayer()
    {
        var player = GameObject.FindWithTag("|Player|").transform;
        transform.position = new Vector3(player.position.x, player.position.y, player.position.z);
    }
}
