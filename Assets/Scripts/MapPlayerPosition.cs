using UnityEngine;

public class MapPlayerPosition : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    private void Update()
    {
        FollowPlayer();
    }
    
    //Update mapPlayerPosition with player position
    private void FollowPlayer()
    {
        var player = GameObject.FindWithTag("|Player|").transform;
        var position = player.position;
        transform.position = new Vector3(position.x, position.y, position.z);
    }
}
