using System;
using UnityEngine;

public class ChaseMovement : IMovementStrategy
{
    // TODO: Find a way to NOT hard code these numbers.
    private const float Speed = 0.6f;
    private const float MaximumTurnRate = 0.7f;
    
    public void MovePlayer(Transform player, PlayerInputActions inputActions)
    {
        var movementInput = inputActions.Player.Move.ReadValue<Vector2>();

        var xAxisInput = movementInput.x;
        var yAxisInput = -1 * movementInput.y;

        if (Math.Abs(xAxisInput) > 0.1f || Math.Abs(yAxisInput) > 0.1f)
        {
            // Squaring the inputs makes finer movements easier.
            var interpretedXInput = xAxisInput * xAxisInput * MaximumTurnRate;
            var interpretedYInput = yAxisInput * yAxisInput * MaximumTurnRate;

            // But squaring negative values makes them positive.
            if (xAxisInput < 0)
            {
                interpretedXInput = -interpretedXInput;
            }
            if (yAxisInput < 0)
            {
                interpretedYInput = -interpretedYInput;
            }

            player.Rotate(interpretedYInput, interpretedXInput, 0, Space.Self);
        }
        
        player.position += player.forward * Speed;
    }
}
