using UnityEngine;

public class ThirdPersonCameraOrbit : MonoBehaviour
{
    [SerializeField] Transform _orbitCenter; 
    [SerializeField] Vector2 _pitchLimits = new Vector2(-40, 75);
    [SerializeField] bool _useCameraDistanceAdjustment = false;
    [SerializeField] float _distance = 4f;
    float _realDistance = 4f;
    [SerializeField] float _lookSensitivity = 2f;

    float _yaw = 0f;
    float _pitch = 10f;

    InputSystem_Actions _input;

    private void Awake()
    {
        _input = new InputSystem_Actions();
        _input.Enable();
        _realDistance = _distance;

        Physics.queriesHitBackfaces = false;
    }

    private void Update()
    {
        if (!_useCameraDistanceAdjustment) { return; }

        Vector2 rayDirection = (transform.position - _orbitCenter.position).normalized;
        if (Physics.Raycast(new Ray(_orbitCenter.position, rayDirection), out RaycastHit hit, _distance))
        {
            Debug.Log("hit");
            _realDistance = hit.distance;
        } 
        else
        {
            _realDistance = _distance;
        }
    }

    private void LateUpdate()
    {
        Vector2 lookInput = _input.Player.Look.ReadValue<Vector2>();

        _yaw += lookInput.x * _lookSensitivity;
        _pitch -= lookInput.y * _lookSensitivity;
        _pitch = Mathf.Clamp(_pitch, _pitchLimits.x, _pitchLimits.y);

        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -_realDistance);

        transform.position = _orbitCenter.position + offset;
        transform.rotation = rotation;
    }
}
