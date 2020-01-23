using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDIsoPlayerMovement : MonoBehaviour
{

    private float _speed = 100f;
    private float _walkSpeed = 0.5f;
    private float _runSpeed = 1f;
 
 
    private float gravity = 8;
 
    private Rigidbody body;
    private Animator anim;
 
    private Vector3 direction;
    float percent;
 
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
 
    private void FixedUpdate()
    {
        Move();
 
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
     
        // left controll is speed up, delete this code 
        if (Input.GetButton("Fire1"))
        {
            percent = _runSpeed * direction.magnitude;
            _speed = 200f;
        }
        else
        {
            percent = _walkSpeed * direction.magnitude;
            _speed = 100f;
        }
 
        //CONVERT direction from local to world relative to camera
        body.velocity = Camera.main.transform.TransformDirection(direction) * _speed * Time.deltaTime;
    }
 
    public void HandleRotation()
    {
        float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        Quaternion lookAt = Quaternion.Slerp(transform.rotation,
                                      Quaternion.Euler(0,targetRotation,0),
                                      0.5f);
        body.rotation = lookAt;
 
    }
 



}
