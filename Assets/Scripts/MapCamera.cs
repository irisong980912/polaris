using System;
using UnityEngine;

public sealed class MapCamera : MonoBehaviour
{
    public Transform mapTarget;
    public float mapHeight = 30f;
    public float mapDistance = 30f;
    public float mapAngle = 45f;
    
    public GameObject playerCamera;
    private IsometricCamera _camHandler;
    private static bool _mapActive;

    private bool MapActive => _camHandler.IsometricCameraActive;
    public static event Action<bool> OnMapSwitch;
    
    private void Start()
    {
        _camHandler = new IsometricCamera(playerCamera, gameObject);
        SetMapCameraLocation();
        
        //Notify ThirdPersonPlayer on map status
        OnMapSwitch?.Invoke(MapActive);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetButtonDown("Fire3")) return;
        SetMapCameraLocation();
        _camHandler.SwitchPerspective();
        OnMapSwitch?.Invoke(MapActive);
    }

    private void SetMapCameraLocation()
    {
        //Exit if no target
        if(!mapTarget)
        {
            return;
        }

        //Build world position vector
        var worldPosition = (Vector3.forward * -mapDistance) + (Vector3.up * mapHeight);

        //Build rotated vector
        var rotatedVector = Quaternion.AngleAxis(mapAngle, Vector3.up) * worldPosition;

        //Move the position
        var flatTargetPosition = mapTarget.position;
        flatTargetPosition.y = 0f;
        var finalPosition = flatTargetPosition + rotatedVector;
        
        _camHandler.SetIsometricCameraLocation(finalPosition, flatTargetPosition - finalPosition);
    }

}
