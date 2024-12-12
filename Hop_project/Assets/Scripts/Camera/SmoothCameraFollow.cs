using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Target and Smoothness")]
    [SerializeField] private Transform target;       // The object to follow (e.g., the ball)
    [SerializeField] private float smoothSpeed = 5f; // Smoothness factor (higher = faster)
    [SerializeField] private float horizontalSmoothSpeed = 10f; // Horizontal smoothing factor

    private Vector3 _offset; // Offset calculated at runtime

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("SmoothCameraFollow: Target is not assigned!");
            return;
        }

        // Calculate initial offset based on camera's position relative to the target
        _offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate desired position using the calculated offset
        Vector3 desiredPosition = target.position + _offset;

        // Smoothly interpolate the camera's position horizontally
        float smoothedX = Mathf.Lerp(transform.position.x, desiredPosition.x, horizontalSmoothSpeed * Time.deltaTime);

        // Use the ball's current Y and Z position directly to avoid unnecessary smoothing (this avoids jitter)
        float smoothedY = Mathf.Lerp(transform.position.y, desiredPosition.y, smoothSpeed * Time.deltaTime);
        float smoothedZ = Mathf.Lerp(transform.position.z, desiredPosition.z, smoothSpeed * Time.deltaTime);

        // Apply the smoothed position to the camera
        transform.position = new Vector3(smoothedX, smoothedY, smoothedZ);
    }
}
