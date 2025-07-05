using ECM.Components;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sittable : Interactable
{
    [SerializeField] float _camHeight;
    CharacterMovement _movementController;
    Rigidbody _playerRigidBody;
    float _previousMaxLatteralSpeed;
    float _previousMaxRiseSpeed;
    Vector3 _previousPosition;
    Vector3 _previousCamPos;

    InputSystem_Actions _inputActions;

    public void Awake()
    {
        _movementController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
        _playerRigidBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        _inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    protected override void Interact()
    {
        base.Interact();

        _inputActions.Player.Sprint.started += EscapePressed;
        
        _previousMaxLatteralSpeed = _movementController.maxLateralSpeed;
        _previousMaxRiseSpeed = _movementController.maxRiseSpeed;
        _movementController.maxLateralSpeed = 0;
        _movementController.maxRiseSpeed = 0;

        //_playerRigidBody.isKinematic = true;

        _previousPosition = _playerRigidBody.transform.position;
        _playerRigidBody.transform.position = transform.position;

        _previousCamPos = Camera.main.transform.localPosition;
        Camera.main.transform.localPosition = new Vector3(_previousCamPos.x, _camHeight, _previousCamPos.z);
    }

    public void EscapePressed(InputAction.CallbackContext ctx)
    {
        _inputActions.Player.Sprint.started -= EscapePressed;
        
        _movementController.maxLateralSpeed = _previousMaxLatteralSpeed;
        _movementController.maxRiseSpeed = _previousMaxRiseSpeed;

        //_playerRigidBody.isKinematic = false;
        Camera.main.transform.localPosition = _previousCamPos;
        _playerRigidBody.transform.position = _previousPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * _camHeight);
        Gizmos.DrawWireSphere(transform.position + Vector3.up * _camHeight, 0.05f);
    }
}
