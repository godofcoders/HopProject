using UnityEngine;
using System.Collections.Generic;
using Ball.States;
using Ball.Movements;

public class ActiveState : IBallState
{
    private List<IMovementBehavior> _movements;
    public ActiveState(List<IMovementBehavior> movements)
    {
        _movements = movements;
    }
    public void HandleInput() { }

    public void UpdateState() 
    {
        foreach (var movement in _movements)
        {
            movement.UpdateMovement();
        }
    }
}
