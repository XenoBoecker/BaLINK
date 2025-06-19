using System;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactionRange = 3f; // Range within which the player can interact with objects
    [SerializeField] private float interactionRadius = 1f; // Radius of the interaction sphere
    [SerializeField] private LayerMask interactionLayer;

    [SerializeField] Transform equipWeaponPoint; // Point where the equipped item will be positioned
    [SerializeField] float equipDuration = 0.5f; // Duration for equipping the item
    private EquippedItem _equippedItem; // Reference to the currently equipped item, if any

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
        if(_inputActions.Player.Interact.WasPressedThisFrame())
        {
            if(_equippedItem != null)
            {
                print("Using equipped item...");
                _equippedItem.UseItem(); // Use the currently equipped item
                return; // Exit early if an item is used
            }
            else
            {
                print("Trying to interact...");
                Interactable interactable = GetInteractable();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }
    private Interactable GetInteractable()
    {
        Physics.SphereCast(transform.position, interactionRadius, transform.forward, out RaycastHit hit, interactionRange, interactionLayer);

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

    internal void EquipItem(EquippedItem item)
    {
        StartCoroutine(EquipItemCoroutine(item));
    }

    private System.Collections.IEnumerator EquipItemCoroutine(EquippedItem item)
    {
        if (_equippedItem != null)
        {
            // Optionally, you can add logic to unequip the current item
            Debug.Log("Unequipping current item: " + _equippedItem.gameObject.name);
            _equippedItem = null; // Clear the currently equipped item
        }

        item.GetComponent<Collider>().enabled = false;

        for (float i = 0; i < equipDuration; i+= Time.deltaTime)
        {
            // Simulate equipping animation or logic here
            item.transform.position = Vector3.Lerp(item.transform.position, equipWeaponPoint.position, i / equipDuration);

            item.transform.rotation = Quaternion.Lerp(item.transform.rotation, equipWeaponPoint.rotation, i / equipDuration);

            yield return null;
        }

        item.transform.position = equipWeaponPoint.position; // Ensure the item is positioned correctly
        item.transform.rotation = equipWeaponPoint.rotation; // Ensure the item is oriented correctly

        item.transform.parent = equipWeaponPoint; // Parent the item to the equip point for proper positioning

        _equippedItem = item; // Set the new equipped item
        Debug.Log("Equipped new item: " + _equippedItem.gameObject.name);
    }
}
