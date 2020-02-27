using UnityEngine;

public class IsometricCamera
{
    // The map must update playerCamera whenever the player enters or exits a puzzle area, as the perspective to
    // return to after closing the map would change if a player is inside a puzzle area.
    public readonly GameObject PlayerCamera;
    public readonly GameObject IsometricCam;

    public bool IsometricCameraActive;

    public IsometricCamera(GameObject playerCamera, GameObject isometricCam)
    {
        PlayerCamera = playerCamera;
        IsometricCam = isometricCam;
        
        PlayerCamera.GetComponent<Camera>().enabled = true;
        IsometricCam.GetComponent<Camera>().enabled = false;
    }

    public void SetIsometricCameraLocation(Vector3 cameraLocation, Vector3 directionOfView)
    {
        IsometricCam.transform.position = cameraLocation;
        IsometricCam.transform.forward = directionOfView;
    }

    public void SwitchPerspective()
    {
        if (IsometricCameraActive)
        {
            IsometricCameraActive = false;
            PlayerCamera.GetComponent<Camera>().enabled = true;
            IsometricCam.GetComponent<Camera>().enabled = false;
        }
        else
        {
            IsometricCameraActive = true;
            PlayerCamera.GetComponent<Camera>().enabled = false;
            IsometricCam.GetComponent<Camera>().enabled = true;
        }
        
    }

    public GameObject GetActiveCamera()
    {
        return IsometricCameraActive ? IsometricCam : PlayerCamera;
    }
    
}
