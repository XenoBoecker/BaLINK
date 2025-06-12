using UnityEngine;
using UnityEngine.Windows;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactionRange = 3f; // Range within which the player can interact with objects

    InputSystem_Actions _inputActions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Enable(); // Enable the player input actions


    }

    private void OnEnable()
    {
        _inputActions.Player.Enable(); // Ensure the player input actions are enabled when the script is enabled
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable(); // Disable the player input actions when the script is disabled
    }

    private void Update()
    {
        print("aouheofo");
        if(_inputActions.Player.Interact.WasPressedThisFrame())
        {
            print("Trying to interact...");
            Interactable interactable = GetInteractable();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
    private Interactable GetInteractable()
    {
        Physics.SphereCast(transform.position, 1f, transform.forward, out RaycastHit hit, interactionRange);

        print("Hit: " + hit.collider?.name);

        if (hit.collider != null)
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                return interactable;
            }
        }

        return null;
    }
}