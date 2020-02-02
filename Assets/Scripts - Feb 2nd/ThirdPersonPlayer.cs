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
        //testOrbit();

        if (_isCollide)
        {
            StartOrbit(_starCol);

        }
        if (Input.GetKey(KeyCode.R))
        {
            Debug.Log("KeyCode down: R" );
            _isCollide = false;

            cam.GetComponent<ThirdPersonCamera>().BreakFree(_starCol);

        }
            

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

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name != "starOrbit") return;
        _starCol = col;
        _isCollide = true;
        Debug.Log("collide");
        //startOrbit(col);


        cam.GetComponent<ThirdPersonCamera>().CollisionDetected(_starCol);
    }

    private void StartOrbit(Collision col)
    {

        var starPos = col.transform.position;
        var playerPos = transform.position;

        // get the camera position in world axis
        //Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        var dir = playerPos - starPos;

        circleRadius = Vector3.Distance(playerPos, starPos);


        //float x = starPos.x + dir.normalized.x ;
        //float y = starPos.y + dir.normalized.y ;
        //float z = starPos.z + dir.normalized.z;
        var x = starPos.x + (Mathf.Cos(_yTimeCounter)) * circleRadius;
        var y = starPos.y + (Mathf.Sin(_xTimeCounter)) * circleRadius;
        var z = starPos.z + dir.normalized.z;


        //float x = starPos.x + Mathf.Cos(timeCounter) * circle_radius;
        //float y = starPos.y + Mathf.Sin(timeCounter) * circle_radius;
        //float z = starPos.z;


        //float x = Mathf.Cos(timeCounter) * starPos.x;
        //float y = Mathf.Sin(timeCounter) * starPos.y;
        //float z = starPos.z;

        var timePos = new Vector3(x, y, z);

        //Vector3 new_pos = starPos + (dir.normalized * Mathf.Cos(timeCounter));
        var newPos = timePos;
        transform.position = newPos;

        _xTimeCounter += Time.deltaTime;
        _yTimeCounter += Time.deltaTime;

       



    }



}
