using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform character;          // the player to look at
    public Transform cam;             // camera itself

    private Transform _planet;

    // set the min and max camera angle on X and Y axis
    public float yAngleMin = -80.0f;  // bottom degree
    public float yAngleMax = 80.0f;   // top degree
    public float smoothSpeed = 0.125f;
    public float distance = 0.8f;

    private float _currentX;
    private float _currentY;

    private Transform _target;

    private bool _orbitDetected;
    private bool _isCancel;

    private bool _levelCleared;

    public Transform TopViewCamPos;

    /// <summary>
    /// Camera Starting Position, creating a zoom in effect
    /// </summary>
    private void Start()
    {
        _target = character;
        _orbitDetected = false;

        var dir = new Vector3(0, 0, -10.0f);
        var rotation = Quaternion.Euler(_currentY, _currentX, 0);
        transform.position = character.position + rotation * dir;

        ClearLevel.OnLevelClear += OnLevelClear;
    }

    private void Update()
    {
        if (_planet)
        {
            print("_orbitDetected true");
            smoothSpeed = 0.8f;
            cam.LookAt(_planet);

        }

        // move the camera angle by mouse
        _currentX += Input.GetAxis("Mouse X");
        _currentY += Input.GetAxis("Mouse Y");

        // unity clamp API ensures that the value is always within the range
        _currentY = Mathf.Clamp(_currentY, yAngleMin, yAngleMax);
    }


    private void LateUpdate()
    {
        // TODO: Test the camera smooth speed when player orbiting the planet
        if (_planet)
        {
            print("_orbitDetected true");
            smoothSpeed = 0.8f;
            cam.LookAt(_planet);

        }
        else if (!_planet)
        {
            
            // set back to default
            smoothSpeed = 0.02f;
            cam.LookAt(_target);

        }

        //if (_levelCleared) return;

        // Camera smooth movement can be only realized in a update() function
        if (_levelCleared)
        {
            Vector3 desiredPosition = TopViewCamPos.position;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.0125f);

            Quaternion newRot = Quaternion.Euler(90, -45, -500); 
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 0.0125f);
        } else
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


    public void OrbitDetected(Transform planet)
    {
        _planet = planet;
        _orbitDetected = true;
        Debug.Log("OrbitDetected");
    }


    public void CancelFocus()
    {
        _planet = null;
        _target = character;
        _orbitDetected = false;
        Debug.Log("CancelFocus");
    }
    
    private void OnLevelClear()
    {
        _levelCleared = true;
        Debug.Log("Camera -- OnLevelClear");
    }


}
