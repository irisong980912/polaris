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

// <<<<<<< fixCam
    private bool _orbitDetected;
    private bool _firstTimeOrbit;
    private bool _firstTimeShot;

    private bool _levelCleared;

    private bool _onSlingShot;

    public Transform topViewCamPos;
// =======
//     private bool _levelCleared;

//     public Transform topViewCamPos;
// >>>>>>> master

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    /// <summary>
    /// Camera Starting Position, creating a zoom in effect
    /// </summary>
    private void Start()
    {


        Orbit.OnOrbit += OnOrbit;
        Orbit.OffOrbit += OffOrbit;
        Orbit.OnSlingShot += OnSlingShot;


        _target = character;

        var dir = new Vector3(0, 0, -10.0f);
        var rotation = Quaternion.Euler(_currentY, _currentX, 0);
        transform.position = character.position + rotation * dir;

        //Orbit.OnOrbitStart += OrbitDetected;
        //Orbit.OnOrbitStop += CancelFocus;
        ClearLevel.OnLevelClear += OnLevelClear;
         

        cam.LookAt(character);
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
// <<<<<<< fixCam
// =======
//         cam.LookAt(_target);
// >>>>>>> master

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
// <<<<<<< fixCam

            var dir = new Vector3(-distance, 0.02f, -distance);
            var rotation = Quaternion.Euler(_currentY, _currentX, 0);


            if (_orbitDetected)
            {
                cam.LookAt(character.parent);

                

                if (_firstTimeOrbit)
                {
                    
                    var smoothPosition = Vector3.Lerp(transform.position, transform.position + new Vector3(-distance, 0, -distance), smoothSpeed * Time.deltaTime);
                    transform.position = smoothPosition;

                    _firstTimeOrbit = false;
                } else
                {
                    cam.rotation = rotation;
                }

            }
            else if (_onSlingShot)
            {
                // look at th left side
                var desiredPosition = character.position - character.right * 10;
                
                // smooth following
                var smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
                transform.position = smoothPosition;

                cam.LookAt(character);
            }

            else
            {

                cam.LookAt(character);
                // smooth following
                var desiredPosition = character.position;
                var smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
                transform.position = smoothPosition;

                // prevent camera shaking
                Vector3 newPosition = Vector3.SmoothDamp(transform.position, transform.position + rotation * dir, ref velocity, smoothTime);
                transform.position = newPosition;
            }
// =======
//             // position the camera behind the player by "distance"
//             var dir = new Vector3(0, 0, -distance);
//             var rotation = Quaternion.Euler(_currentY, _currentX, 0);
//             // smooth following
//             var desiredPosition = _target.position;
//             var smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
//             transform.position = smoothPosition + rotation * dir;
// >>>>>>> master
        }
        

        
        
    }

// <<<<<<< fixCam

    private void OnOrbit()
    {
        _orbitDetected = true;
        _firstTimeOrbit = true;
        Debug.Log("OrbitDetected： " + _orbitDetected);
// =======
//     private void OrbitDetected(Transform planet)
//     {
//         // TODO: Test the camera smooth speed when player orbiting the planet
//         smoothSpeed = 0.8f;
//         _target = planet;
//         Debug.Log("OrbitDetected");
//     }
// >>>>>>> master

       

    }

// <<<<<<< fixCam
    private void OffOrbit()
    {
        _orbitDetected = false;
// =======
//     private void CancelFocus()
//     {
//         smoothSpeed = 0.02f;
//         _target = character;
// >>>>>>> master
        Debug.Log("CancelFocus");
    }
    
    private void OnLevelClear()
    {
        _levelCleared = true;
        Debug.Log("Camera -- OnLevelClear");
    }

    private void OnSlingShot()
    {
        _onSlingShot = true;
        _firstTimeShot = true;
        Debug.Log("OnSlingShot");

        Invoke(nameof(OffSlingShot), 2);

    }

    private void OffSlingShot()
    {
        _onSlingShot = false;
        Debug.Log("OnSlingShot");
    }


}
