using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonController_Rigidbody : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform cameraTransform;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Look Settings")]
    [SerializeField] float lookSensitivity = 5f;
    [SerializeField] float rotationSmoothTime = 0.1f;

    Rigidbody rb;
    InputSystem_Actions input;

    float currentYaw;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        input = new InputSystem_Actions();
        input.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable() => input.Enable();
    private void OnDisable() => input.Disable();

    private void FixedUpdate()
    {
        Move();
        Look();
    }

    private void Move()
    {
        Vector2 inputDir = input.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(inputDir.x, 0f, inputDir.y);

        if (move.magnitude > 0.1f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * move.z + camRight * move.x;
            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void Look()
    {
        Vector3 lookForward = cameraTransform.forward;
        lookForward.y = 0;
        lookForward.Normalize();

        if (lookForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookForward);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSmoothTime / Time.fixedDeltaTime));
        }
    }
}
