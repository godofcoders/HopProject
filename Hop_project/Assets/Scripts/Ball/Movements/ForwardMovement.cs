using UnityEngine;

namespace Ball.Movements
{
    public class ForwardMovement : IMovementBehavior
    {
        private readonly Rigidbody _rigidbody;
        private readonly float _tileDistance; // Distance the ball needs to move to reach the next tile
        private bool _isMovingForward;

        private float _forwardVelocity;

        public ForwardMovement(Rigidbody rigidbody, float tileDistance)
        {
            _rigidbody = rigidbody;
            _tileDistance = tileDistance;
            _isMovingForward = false;
        }

        public void StartForwardMovement(float jumpDuration)
        {
            Debug.Log("Forward movement started");
            _forwardVelocity = _tileDistance / jumpDuration;  // Ensure the ball covers the tile distance in the jump duration
            _isMovingForward = true;
        }

        public void StopForwardMovement()
        {
            Debug.Log("Forward movement stopped");
            _isMovingForward = false;
        }

        public void UpdateMovement()
        {
            if (_isMovingForward) 
            {
                Vector3 velocity = _rigidbody.linearVelocity;
                velocity.z = _forwardVelocity; 
                _rigidbody.linearVelocity = velocity;
            }

            if (_rigidbody.linearVelocity.y == 0)  // Ball has landed (vertical velocity is zero)
            {
                // _isMovingForward = false;  // Uncomment if you want to stop forward movement when landing
            }
        }

    }
}
