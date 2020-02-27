using UnityEngine;

public class IsometricCamera
{
    // The map must update playerCamera whenever the player enters or exits a puzzle area, as the perspective to
    // return to after closing the map would change if a player is inside a puzzle area.
    public GameObject PlayerCamera;
    private readonly GameObject _isometricCam;

    public bool IsometricCameraActive;

    public IsometricCamera(GameObject playerCamera, GameObject isometricCam)
    {
        PlayerCamera = playerCamera;
        _isometricCam = isometricCam;
        
        PlayerCamera.GetComponent<Camera>().enabled = true;
        _isometricCam.GetComponent<Camera>().enabled = false;
    }

    public void SetIsometricCameraLocation(Vector3 cameraLocation, Vector3 directionOfView)
    {
        _isometricCam.transform.position = cameraLocation;
        _isometricCam.transform.forward = directionOfView;
    }

    public void SwitchPerspective()
    {
        if (IsometricCameraActive)
        {
            IsometricCameraActive = false;
            PlayerCamera.GetComponent<Camera>().enabled = true;
            _isometricCam.GetComponent<Camera>().enabled = false;
        }
        else
        {
            IsometricCameraActive = true;
            PlayerCamera.GetComponent<Camera>().enabled = false;
            _isometricCam.GetComponent<Camera>().enabled = true;
        }
        
    }

    public GameObject GetActiveCamera()
    {
        return IsometricCameraActive ? _isometricCam : PlayerCamera;
    }
    
}
