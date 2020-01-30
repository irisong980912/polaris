using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayer : MonoBehaviour
{

    public float rotationRate = 360;
    public float moveSpeed = 2;


    private void Update()
    {

        PlayerMovement();

    }

    private void PlayerMovement()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var playerMovement = new Vector3(horizontal, 0f, vertical).normalized * (moveSpeed * Time.deltaTime);
        transform.Translate(playerMovement, Space.Self);
    }


}