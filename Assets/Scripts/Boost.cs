using UnityEngine;

public class Boost : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var playerScript = GameObject.FindWithTag("|Player|").GetComponent<ThirdPersonPlayer>();
        if (playerScript.stardust >= 3)
        {
            playerScript.speed = Input.GetButton("Jump") ? 1.3f : 0.6f;
        }
    }
}
