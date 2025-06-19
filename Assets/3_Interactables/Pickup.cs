using UnityEngine;

[RequireComponent(typeof(EquippedItem))]
public class Pickup : Interactable
{
    public override void Interact()
    {
        base.Interact();

        // Logic for picking up the crossbow can be added here

        PlayerInteractor playerInteractor = FindAnyObjectByType<PlayerInteractor>();
        if (playerInteractor != null)
        {
            // Assuming PlayerInteractor has a method to equip the crossbow
            playerInteractor.EquipItem(GetComponent<EquippedItem>());
            this.enabled = false; // Disable this interactable after picking up
        }
        else
        {
            Debug.LogWarning("PlayerInteractor not found in the scene.");
        }
    }
}