using System;
using UnityEngine;

public class PuzzleCamera : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject puzzleCamera;
    private IsometricCamera _camHandler;

    public static event Action<GameObject> OnPuzzleEnterOrExit;

    private void Start()
    {
        _camHandler = new IsometricCamera(playerCamera, gameObject);
        _camHandler.SetIsometricCameraLocation(puzzleCamera.transform.position, puzzleCamera.transform.forward);
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
