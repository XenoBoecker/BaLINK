using UnityEngine;

public class ThirdPersonCameraOrbit : MonoBehaviour
{
    [SerializeField] Transform target;         // Player's head or upper spine
    [SerializeField] Vector2 pitchLimits = new Vector2(-40, 75);
    [SerializeField] float distance = 4f;
    [SerializeField] float lookSensitivity = 2f;

    float yaw = 0f;
    float pitch = 10f;

    InputSystem_Actions input;

    private void Awake()
    {
        input = new InputSystem_Actions();
        input.Enable();
    }

    private void LateUpdate()
    {
        Vector2 lookInput = input.Player.Look.ReadValue<Vector2>();

        yaw += lookInput.x * lookSensitivity;
        pitch -= lookInput.y * lookSensitivity;
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        transform.position = target.position + offset;
        transform.rotation = rotation;
    }
}
