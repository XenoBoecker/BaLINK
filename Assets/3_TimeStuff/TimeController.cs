using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TimeField))]
public class TimeController : MonoBehaviour
{
    TimeField _timeField;
    InputSystem_Actions _playerInput;

    private void Awake()
    {
        _timeField = GetComponent<TimeField>();

        _playerInput = new InputSystem_Actions();
        _playerInput.Enable();
    }

    private void OnEnable()
    {
        // Register input event
        //playerInput.Player.ToggleTime.performed += ctx => ToggleTime();
        _playerInput.Player.ToggleTime.started += TimeSlow;
        _playerInput.Player.ToggleTime.canceled += TimeSlow;
    }

    private void OnDisable()
    {
        // Unregister input event
        //playerInput.Player.ToggleTime.performed -= ctx => ToggleTime();
        _playerInput.Player.ToggleTime.started -= TimeSlow;
        _playerInput.Player.ToggleTime.canceled -= TimeSlow;
    }

    private void TimeSlow(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _timeField.SetTimeScaleState(true);
        }

        if (ctx.canceled)
        {
            _timeField.SetTimeScaleState(false);
        }
    }
}
