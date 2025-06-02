using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] TimeField timeField;

    InputSystem_Actions playerInput;

    private void Awake()
    {
        playerInput = new InputSystem_Actions();
        playerInput.Enable();
    }

    private void OnEnable()
    {
        // Register input event
        playerInput.Player.ToggleTime.performed += ctx => ToggleTime();
    }

    private void OnDisable()
    {
        // Unregister input event
        playerInput.Player.ToggleTime.performed -= ctx => ToggleTime();
    }

    private void ToggleTime()
    {
        // Example logic to toggle time (assuming TimeField has something like a bool IsPaused)
        if (timeField != null)
        {
            timeField.Toggle(); // Replace with your own method
        }
        else
        {
            Debug.LogWarning("TimeField is not assigned.");
        }
    }
}
