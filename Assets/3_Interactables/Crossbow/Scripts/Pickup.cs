using UnityEngine;

[RequireComponent(typeof(EquippedItem))]
public class Pickup : Interactable
{
    protected override void Interact()
    {
        PlayerInteractor playerInteractor = FindAnyObjectByType<PlayerInteractor>();
        if (playerInteractor != null)
        {
            EquippedItem equippedItem = GetComponent<EquippedItem>();
            playerInteractor.EquipItem(equippedItem);
            equippedItem.SetIsEquipped(true);

            InteractedSuccessfully();
            this.enabled = false;

        }
        else
        {
            Debug.LogWarning("PlayerInteractor not found in the scene.");
        }
    }
}