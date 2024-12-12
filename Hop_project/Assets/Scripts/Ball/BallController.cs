using UnityEngine;
using Ball.Movements;
using System;
using UnityEditor.PackageManager;

public class BallController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float tileDistance = 10f;
    [SerializeField] private float slideSpeed = 10f;
    [SerializeField] private float laneClamp = 3f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;

    private Rigidbody _rigidbody;
    private JumpMovement _jumpMovement;
    private ForwardMovement _forwardMovement;
    private HorizontalMovement _horizontalMovement;

    private bool _isMovementStarted = false; // Flag to check if movement has started
    private bool _isLandedForFirstTime;
    public static Action OnTileHit;
    public static Action OnDeath;
    private Vector3 initialPos;

    void Awake() 
    {
        initialPos= transform.position;
    }

    private void OnEnable()
    {
        UIManager.ResetScore += ResetGame;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = true;

        _forwardMovement = new ForwardMovement(_rigidbody, tileDistance);
        _jumpMovement = new JumpMovement(_rigidbody, jumpForce, this, _forwardMovement);
        _horizontalMovement = new HorizontalMovement(transform, slideSpeed, laneClamp);
    }

    private void Update()
    {
        if (!_isMovementStarted && Input.GetMouseButtonDown(0))
        {
            _isMovementStarted = true;
            Debug.Log("Game Started - Movement Enabled");
        }

        _horizontalMovement.UpdateInput();

        if (gameObject.transform.position.y < -5 && _isMovementStarted) 
        {
            OnDeath?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if (_isMovementStarted)
        {
            _jumpMovement.UpdateMovement();
            _forwardMovement.UpdateMovement();
            _horizontalMovement.UpdateMovement();
        }
    }

    public bool IsGrounded()
    {
        var isGrounded= Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (isGrounded && _isMovementStarted)
        {
            if (_isLandedForFirstTime)
            {
                OnTileHit?.Invoke();
                _isLandedForFirstTime = false;
            }
        }
        else
        {
            _isLandedForFirstTime = true;
        }
        return isGrounded;
    }

    private void ResetGame()
    {
        Debug.Log("Resetting game...");

        // Temporarily freeze Rigidbody physics
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        // Reset the position safely using MovePosition
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        // Use Rigidbody.MovePosition for a proper reset
        _rigidbody.MovePosition(initialPos);

        Debug.Log("Ball position reset to: " + initialPos);

        // Reset the TileSpawner
        TileSpawner tileSpawner = FindFirstObjectByType<TileSpawner>();
        if (tileSpawner != null)
        {
            tileSpawner.ResetTiles();
            Debug.Log("Tiles have been reset.");
        }
        else
        {
            Debug.LogError("TileSpawner not found in the scene!");
        }

        // Unfreeze Rigidbody and re-enable gravity
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.useGravity = true;

        // Reset game movement flag
        _isMovementStarted = false;
        Debug.Log("Game reset completed.");
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * groundCheckDistance);
    }

    private void OnDestroy()
    {
        UIManager.ResetScore -= ResetGame;
    }
}
