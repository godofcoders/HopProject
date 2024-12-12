using UnityEngine;
using Ball.Movements;

public class HorizontalMovement : IMovementBehavior
{
    private Transform _transform;
    private float _slideSpeed;
    private float _laneClamp;
    private Vector3 _targetPosition;

    private float _sensitivityMultiplier = 1.0f;
    private float _horizontalInput; // Stores input data
    private float _smoothTime = 0.8f; // Smooth movement time
    private float _velocityX = 0f; // SmoothDamp helper

    private float _currentInput = 0f; // Current input value for smooth transition

    public HorizontalMovement(Transform transform, float slideSpeed, float laneClamp)
    {
        _transform = transform;
        _slideSpeed = slideSpeed;
        _laneClamp = laneClamp;
        _targetPosition = transform.position;
    }

    public void UpdateInput()
    {
        if (Input.GetMouseButton(0))
        {
            _horizontalInput = Input.GetAxis("Mouse X") * _sensitivityMultiplier;
        }
        else
        {
            _horizontalInput = 0f; // Reset input when mouse is released
        }

        _currentInput = Mathf.Lerp(_currentInput, _horizontalInput, _smoothTime);
    }

    public void UpdateMovement()
    {
        _targetPosition.x += _currentInput * _slideSpeed * Time.fixedDeltaTime;
        _targetPosition.x = Mathf.Clamp(_targetPosition.x, -_laneClamp, _laneClamp);

        // Smoothly move the ball to the target position
        Vector3 position = _transform.position;
        position.x = Mathf.SmoothDamp(position.x, _targetPosition.x, ref _velocityX, _smoothTime);
        _transform.position = position;
    }
}
