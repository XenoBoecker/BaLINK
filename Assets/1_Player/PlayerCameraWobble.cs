using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCameraWobble : MonoBehaviour
{
    [SerializeField] private float _bobStrength;
    [SerializeField] private float _bobFrequency;

    [SerializeField] private float _tiltStrength;
    [SerializeField] private float _tiltFrequency;

    private Rigidbody _rb;
    private Camera _playerCam;
    private Vector3 _initialCamPosition;
    float _dist;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerCam = Camera.main;
        _initialCamPosition = _playerCam.transform.localPosition;
    }

    private void Update()
    {
        _dist += _rb.linearVelocity.magnitude * Time.deltaTime;

        _playerCam.transform.localPosition = _initialCamPosition + new Vector3(0, Mathf.Cos(_dist * _bobFrequency) * _bobStrength, 0);
        _playerCam.transform.eulerAngles = new Vector3(_playerCam.transform.eulerAngles.x, _playerCam.transform.eulerAngles.y, Mathf.Cos(_dist * _tiltFrequency) * _tiltStrength);
    }
}
