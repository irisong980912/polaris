using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDIsoPlayerMovement : MonoBehaviour
{

    private float _speed = 100f;
    private float _walkSpeed = 0.5f;
    private float _runSpeed = 1f;

 
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
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");
        var zAxis = Input.GetAxis("Jump");
 
        direction = new Vector3(xAxis, yAxis, zAxis);
 
        direction = direction.normalized;

        /// _body.AddForce(direction * _speed);
     
        /// left controll is speed up, delete this code 
        if (Input.GetButton("Fire1"))
        {
            _speed = 200f;
        }
        else
        {
            _speed = 100f;
        }
 
        /// CONVERT direction from local to world relative to camera
        _body.velocity = Camera.main.transform.TransformDirection(direction) * _speed * Time.deltaTime;
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
