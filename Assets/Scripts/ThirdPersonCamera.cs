using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform lookAt;          // the player to look at
    public Transform cam;             // camera itself

    // set the min and max camera angle on X and Y axis
    public float yAngleMin = -80.0f;  // bottom degree
    public float yAngleMax = 80.0f;   // top degree

    private const float Distance = 5.0f;
    private float _currentX;
    private float _currentY;

    private Collision starCol = null;
    private bool isCollide = false;


    private void Update()
    {
        // move the camera angle by mouse
        _currentX += Input.GetAxis("Mouse X");
        _currentY += Input.GetAxis("Mouse Y");

        // unity clamp API ensures that the value is always within the range
        _currentY = Mathf.Clamp(_currentY, yAngleMin, yAngleMax);
    }
    private void LateUpdate()
    {
        
        if (isCollide)
        {
            Debug.Log("collide cam true");

            cam.LookAt(starCol.transform.position);

        } else
        {
            // position the camera behind the player by "distance"
            var dir = new Vector3(0, 0, -Distance);

            var rotation = Quaternion.Euler(_currentY, _currentX, 0);
            // transform camera position
            var position = lookAt.position;
            cam.position = position + rotation * dir;
            // keep the lookAt target at the center of the camera (camera follows target)

            cam.LookAt(position);
        }
            
    }

    public void CollisionDetected(Collision col)
    {
    
        starCol = col;
        isCollide = true;
        Debug.Log("collide cam");
     
    }

    public void BreakFree(Collision col)
    {

        starCol = col;
        isCollide = false;
        Debug.Log("collide cam");

    }

}
