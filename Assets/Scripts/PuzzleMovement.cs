using UnityEngine;

public class PuzzleMovement : MonoBehaviour
{
    private bool _isInPuzzle;
    private GameObject _currentPuzzle;

    private void Start()
    {
        PuzzleCamera.OnPuzzleEnterOrExit += OnPuzzleEnterOrExit;
    }

    // Update is called once per frame
    private void Update()
    {
        var xAxis = Input.GetAxisRaw("Horizontal");
        var yAxis = Input.GetAxisRaw("Vertical");
        var movement = new Vector3(xAxis, 0, yAxis).normalized;
        
        gameObject.transform.Translate(movement, _currentPuzzle.transform);
    }

    private void OnPuzzleEnterOrExit(GameObject activeCamera, GameObject puzzleCameraContainer)
    {
        if (_isInPuzzle)
        {
            _isInPuzzle = false;
            gameObject.GetComponent<ThirdPersonPlayer>().enabled = true;
            gameObject.GetComponent<PuzzleMovement>().enabled = false;
        }
        else
        {
            _currentPuzzle = puzzleCameraContainer;
            _isInPuzzle = true;
            gameObject.GetComponent<ThirdPersonPlayer>().enabled = false;
            gameObject.GetComponent<PuzzleMovement>().enabled = true;
        }
    }
}
