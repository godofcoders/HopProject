using UnityEngine;
using Ball.Movements;

public class JumpMovement : IMovementBehavior
{
    private readonly Rigidbody _rigidbody;
    private readonly float _jumpForce;
    private readonly BallController _controller;
    private readonly ForwardMovement _forwardMovement;

    public JumpMovement(Rigidbody rigidbody, float jumpForce, BallController controller, ForwardMovement forwardMovement)
    {
        _rigidbody = rigidbody;
        _jumpForce = jumpForce;
        _controller = controller;
        _forwardMovement = forwardMovement;
    }

    public void UpdateMovement()
    {
        if (_controller.IsGrounded()) // Ensure the ball is on the ground
        {
            if (_rigidbody.linearVelocity.y < 0.1f) // Only trigger jump when grounded and not already jumping
            {
                Debug.Log("Appying jump");
                float verticalVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * _jumpForce);
                _rigidbody.linearVelocity = new Vector3(0, verticalVelocity, 0);
                float jumpDuration = (2 * verticalVelocity) / Mathf.Abs(Physics.gravity.y);
                _forwardMovement.StartForwardMovement(jumpDuration); // Ensure the ball moves forward during the jump
            }
            else
            {
                Debug.Log("velocity not zero");
                // Keep applying forward velocity after jump is initiated
                _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, _rigidbody.linearVelocity.z);
            }
        }
    }

}
