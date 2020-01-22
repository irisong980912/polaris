using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDIsoPlayerMovement : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var zAxisPlus = Input.GetKey("q");
        var zAxisMinus = Input.GetKey("space");

        var zAxis = 0.0f;

        if (zAxisPlus)
        {
            zAxis = 1.0f;
        }
        else if (zAxisMinus)
        {
            zAxis = -1.0f;
        }
        
        var playerMovement = new Vector3(
                                 horizontal,
                                 vertical,
                                 zAxis
                                 ).normalized * (speed * Time.deltaTime);
        transform.Translate(playerMovement, Space.Self);
    }
}
