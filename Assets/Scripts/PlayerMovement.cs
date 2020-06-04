using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputActions _inputAction;
    private IMovementStrategy _movementStrategy;

    private void Awake()
    {
        _inputAction = new PlayerInputActions();
        _movementStrategy = new ChaseMovement();
    }

    private void OnEnable()
    {
        _inputAction.Enable();
    }

    private void OnDisable()
    {
        _inputAction.Disable();
    }

    private void FixedUpdate()
    {
        _movementStrategy.MovePlayer(transform, _inputAction);
    }
}
