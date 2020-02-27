using System;
using UnityEngine;

public class PuzzleCamera : MonoBehaviour
{
    public GameObject playerCamera;
    private IsometricCamera _camHandler;

    public static event Action<GameObject> OnPuzzleEnterOrExit;

    private void Start()
    {
        var self = transform;
        _camHandler = new IsometricCamera(playerCamera, gameObject);
        _camHandler.SetIsometricCameraLocation(self.position, self.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        _camHandler.SwitchPerspective();
        OnPuzzleEnterOrExit?.Invoke(_camHandler.GetActiveCamera());
    }

    private void OnTriggerExit(Collider other)
    {
        _camHandler.SwitchPerspective();
        OnPuzzleEnterOrExit?.Invoke(_camHandler.GetActiveCamera());
    }
}
