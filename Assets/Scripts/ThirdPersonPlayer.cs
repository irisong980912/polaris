using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayer : MonoBehaviour
{

    public float speed = 200f;
    public float runSpeed = 300f;
    public float circle_radius = 1f;

    public Transform cam; // camera itself
    public Transform star; // camera itself

    private Rigidbody _body;
 
    private Vector3 direction;

    private Collision starCol = null;
    private float xTimeCounter = 0;
    private float yTimeCounter = Mathf.PI;

    private bool isCollide = false;
 
    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }
 
    private void FixedUpdate()
    {
        //testOrbit();

        if (isCollide)
        {
            startOrbit(starCol);

        }
        if (Input.GetKey(KeyCode.R))
        {
            Debug.Log("KeyCode down: R" );
            isCollide = false;

            cam.GetComponent<ThirdPersonCamera>().BreakFree(starCol);

        }
            

        Move();

        /// if the mc is not at the same location as the camera, 
        /// handle camera rotation
        if (direction != Vector3.zero)
        {
            HandleRotation();
        }

        // if collide 

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

    public void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name == "starOrbit")
        {
            starCol = col;
            isCollide = true;
            Debug.Log("collide");
            //startOrbit(col);


            cam.GetComponent<ThirdPersonCamera>().CollisionDetected(starCol);
        }
    }

    public void startOrbit(Collision col)
    {

        Vector3 starPos = col.transform.position;
        Vector3 playerPos = transform.position;

        // get the camera position in world axis
        //Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        Vector3 dir = playerPos - starPos;

        circle_radius = Vector3.Distance(playerPos, starPos);


        //float x = starPos.x + dir.normalized.x ;
        //float y = starPos.y + dir.normalized.y ;
        //float z = starPos.z + dir.normalized.z;
        float x = starPos.x + (Mathf.Cos(yTimeCounter)) * circle_radius;
        float y = starPos.y + (Mathf.Sin(xTimeCounter)) * circle_radius;
        float z = starPos.z + dir.normalized.z;


        //float x = starPos.x + Mathf.Cos(timeCounter) * circle_radius;
        //float y = starPos.y + Mathf.Sin(timeCounter) * circle_radius;
        //float z = starPos.z;


        //float x = Mathf.Cos(timeCounter) * starPos.x;
        //float y = Mathf.Sin(timeCounter) * starPos.y;
        //float z = starPos.z;

        Vector3 timePos = new Vector3(x, y, z);

        //Vector3 new_pos = starPos + (dir.normalized * Mathf.Cos(timeCounter));
        Vector3 new_pos = timePos;
        transform.position = new_pos;

        xTimeCounter += Time.deltaTime;
        yTimeCounter += Time.deltaTime;

       



    }



}
