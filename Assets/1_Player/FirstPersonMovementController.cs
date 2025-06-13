using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _cameraTransform;

    [Header("Movement Settings")]
    [SerializeField] float _maxSpeed = 5f;
    [SerializeField] float _acceleration = 0.1f;
    [Tooltip("Percentage speed reduction of the player when not moving")]
    [SerializeField] float _deceleration = 0.95f;
    [SerializeField] private float wallDetectionRange = 0.51f;
    Vector3 _moveDir;

    [Header("Jump Settings")]
    [SerializeField] float _jumpHeight = 1f;
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _groundCheckDistance = 0.1f;
    bool _isGrounded;

    [Header("Look Settings")]
    [SerializeField] float _lookSensitivity = 5f;
    [SerializeField] float _rotationSmoothTime = 0.1f;
    float _cameraPitch;

    Rigidbody _rb;
    InputSystem_Actions _input;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _input = new InputSystem_Actions();
        _input.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void FixedUpdate()
    {
        Jump();
        Move();
        Look();
    }

    private void Jump()
    {
        _isGrounded = Physics.CheckSphere(transform.position, _groundCheckDistance, _groundLayer);

        if (_isGrounded && _input.Player.Jump.triggered)
        {
            _rb.linearVelocity += Vector3.up * Mathf.Sqrt(2f * _jumpHeight * Mathf.Abs(_gravity)); // Calculate jump velocity based on jump height and gravity
        }

        if (!_isGrounded)
        {
            _rb.linearVelocity += Vector3.up * _gravity * Time.fixedDeltaTime;
        }

        // Reset vertical velocity when grounded
        if (_isGrounded && _rb.linearVelocity.y < 0f)
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        }
    }

    private void Move()
    {
        Vector2 inputDir = _input.Player.Move.ReadValue<Vector2>();
        Vector2 move = new Vector2(inputDir.x, inputDir.y);

        Vector3 horizontalVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        if (move.magnitude > 0.1f)
        {
            move = move.normalized;
            _moveDir = transform.forward * move.y + transform.right * move.x;
            _moveDir.y = 0f;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, _moveDir.normalized, out hit, wallDetectionRange, _groundLayer))
            {
                _moveDir = Vector3.ProjectOnPlane(_moveDir, hit.normal);
            }

            horizontalVelocity += _moveDir * _acceleration * Time.fixedDeltaTime;
        }
        else
        {
            horizontalVelocity *= _deceleration;
        }

        // Clamp horizontal speed
        if (horizontalVelocity.magnitude > _maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * _maxSpeed;
        }

        // Apply velocity with preserved Y (vertical) component
        _rb.linearVelocity = new Vector3(horizontalVelocity.x, _rb.linearVelocity.y, horizontalVelocity.z);
    }

    private void Look()
    {
        Vector2 lookInput = _input.Player.Look.ReadValue<Vector2>();

        float mouseX = lookInput.x * _lookSensitivity;
        float mouseY = lookInput.y * _lookSensitivity;

        // Rotate player left/right (yaw)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera up/down (pitch)
        _cameraPitch -= mouseY;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -89f, 89f); // Prevent flipping

        _cameraTransform.localEulerAngles = new Vector3(_cameraPitch, 0f, 0f);
    }

}
