using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonMovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _cameraTransform;

    [Header("Movement Settings")]
    [SerializeField] float _maxSpeed = 5f;
    [SerializeField] float _acceleration = 0.1f;
    [Tooltip("Percentage speed reduction of the player when not moving")]
    [SerializeField] float _deceleration = 0.95f;
    float _currentSpeed = 0f;
    Vector3 _moveDir;

    [Header("Look Settings")]
    [SerializeField] float _lookSensitivity = 5f;
    [SerializeField] float _rotationSmoothTime = 0.1f;

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
        Move();
        Look();
    }

    private void Move()
    {
        Vector2 inputDir = _input.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(inputDir.x, 0f, inputDir.y);

        if (move.magnitude > 0.1f)
        {
            //makes sure that the movement speed in any direction stays the same
            move = move.normalized;

            Vector3 camForward = _cameraTransform.forward;
            Vector3 camRight = _cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            //calculate the movement direction based on camera view and player inputs
            _moveDir = camForward * move.z + camRight * move.x;

            //accelerate every frame
            _currentSpeed += _acceleration * Time.deltaTime;
            //cap the speed to max movement speed
            if (_currentSpeed > _maxSpeed) { _currentSpeed = _maxSpeed; }
        } 
        else
        {
            _currentSpeed *= _deceleration;
        }

        _rb.MovePosition(_rb.position + _moveDir * _currentSpeed * Time.fixedDeltaTime);
    }

    private void Look()
    {
        Vector3 lookForward = _cameraTransform.forward;
        lookForward.y = 0;
        lookForward.Normalize();

        if (lookForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookForward);
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, _rotationSmoothTime / Time.fixedDeltaTime));
        }
    }
}
