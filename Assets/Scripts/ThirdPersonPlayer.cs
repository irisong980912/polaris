using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayer : MonoBehaviour
{

    public float speed = 200f;
    public float runSpeed = 300f;

    public Transform cam; // camera itself

    public int stardust = 0;
 
    private Rigidbody _body;
 
    private Vector3 direction;
 
    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }
 
    private void FixedUpdate()
    {
        Move();
 
        /// if the mc is not at the same location as the camera, 
        /// handle camera rotation
        if (direction != Vector3.zero)
        {
           HandleRotation();
        }

    }
 
    public void Move()
    {
        // x is left and right
        // y is in and out
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");
        // var zAxis = Input.GetAxis("Jump");
 
        direction = new Vector3(xAxis, 0f, yAxis);
 
        direction = direction.normalized;
     
        /// left controll is speed up, delete this code 
        if (Input.GetButton("Fire1"))
        {
            speed = runSpeed;
        }

        /// Camera.main to cam
        /// Convert direction from local to world relative to camera
        _body.velocity = cam.transform.TransformDirection(direction) * speed * Time.deltaTime;

    }
 
    public void HandleRotation()
    {
        /// determines which angle that the camera is looking at
        float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        Quaternion lookAt = Quaternion.Slerp(transform.rotation,
                                      Quaternion.Euler(0,targetRotation,0),
                                      0.5f);
        _body.rotation = lookAt;
 
    }

}
