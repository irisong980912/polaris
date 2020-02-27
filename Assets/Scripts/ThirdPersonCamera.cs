using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform character;           // the player to look at
    public Transform cam;                 // camera itself

    // set the min and max camera angle on X and Y axis
    public float yAngleMin = -80.0f;      // bottom degree
    public float yAngleMax = 80.0f;       // top degree
    public float smoothSpeed = 0.125f;
    public float distance = 0.8f;

    private float _currentX;
    private float _currentY;

    private Transform _target;

    private bool _levelCleared;

    public Transform topViewCamPos;

    /// <summary>
    /// Camera Starting Position, creating a zoom in effect
    /// </summary>
    private void Start()
    {
        _target = character;

        var dir = new Vector3(0, 0, -10.0f);
        var rotation = Quaternion.Euler(_currentY, _currentX, 0);
        transform.position = character.position + rotation * dir;

        Orbit.OnOrbitStart += OrbitDetected;
        Orbit.OnOrbitStop += CancelFocus;
        ClearLevel.OnLevelClear += OnLevelClear;
    }

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
        cam.LookAt(_target);

        // Camera smooth movement can be only realized in a update() function
        if (_levelCleared)
        {
            var desiredPosition = topViewCamPos.position;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.0125f);

            var newRot = Quaternion.Euler(90, -45, -500); 
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 0.0125f);
        } 
        else
        {
            // position the camera behind the player by "distance"
            var dir = new Vector3(0, 0, -distance);
            var rotation = Quaternion.Euler(_currentY, _currentX, 0);
            // smooth following
            var desiredPosition = _target.position;
            var smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothPosition + rotation * dir;
        }
        
    }

    private void OrbitDetected(Transform planet)
    {
        // TODO: Test the camera smooth speed when player orbiting the planet
        smoothSpeed = 0.8f;
        _target = planet;
        Debug.Log("OrbitDetected");
    }


    private void CancelFocus()
    {
        smoothSpeed = 0.02f;
        _target = character;
        Debug.Log("CancelFocus");
    }
    
    private void OnLevelClear()
    {
        _levelCleared = true;
        Debug.Log("Camera -- OnLevelClear");
    }
    
}
