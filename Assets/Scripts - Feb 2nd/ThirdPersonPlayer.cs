using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThirdPersonPlayer : MonoBehaviour
{

    public float speed = 200f;
    public float circleRadius = 1f;
    
    public int stardust = 0;

    public List<GameObject> inventory = new List<GameObject>();
    
    public Transform cam; // camera itself

    private Rigidbody _body;
 
    private Vector3 _direction;

    private Collision _starCol;
    private float _xTimeCounter;
    private float _yTimeCounter = Mathf.PI;

    private bool _isCollide;
 
    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }
 
    private void FixedUpdate()
    {
            
        Move();

        // if the mc is not at the same location as the camera,
        // handle camera rotation
        if (_direction != Vector3.zero)
        {
            HandleRotation();
        }

        // if collide 

    }

    private void Move()
    {
        // x is left and right
        // y is in and out
        var xAxis = Input.GetAxisRaw("Horizontal");
        var yAxis = Input.GetAxisRaw("Vertical");
        // var zAxis = Input.GetAxis("Jump");
 
        _direction = new Vector3(xAxis, 0f, yAxis);
 
        _direction = _direction.normalized;
        
        // Camera.main to cam
        // Convert direction from local to world relative to camera
        _body.velocity = cam.transform.TransformDirection(_direction) * (speed * Time.deltaTime);

    }

    private void HandleRotation()
    {
        // determines which angle that the camera is looking at
        var targetRotation = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        Quaternion lookAt = Quaternion.Slerp(transform.rotation,
                                      Quaternion.Euler(0,targetRotation,0),
                                      0.5f);
        _body.rotation = lookAt;
 
    }

}
