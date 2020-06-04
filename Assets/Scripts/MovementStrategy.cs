using UnityEngine;

internal interface IMovementStrategy
{
    void MovePlayer(Transform player, PlayerInputActions inputActions);
}