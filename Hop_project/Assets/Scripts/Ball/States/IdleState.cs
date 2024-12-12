using UnityEngine;
using Ball.States;
public class IdleState : IBallState
{
    private System.Action _onStartMovement;

    public IdleState(System.Action onStartMovement) 
    {
        _onStartMovement = onStartMovement;
    }
    public void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            _onStartMovement?.Invoke();
        }
    }

    public void UpdateState()
    {
   
    }
}
