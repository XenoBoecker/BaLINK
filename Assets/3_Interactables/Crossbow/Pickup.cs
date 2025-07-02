using UnityEngine;

[RequireComponent(typeof(EquippedItem))]
public class Pickup : Interactable
{
    public override void Interact()
    {
        PlayerInteractor playerInteractor = FindAnyObjectByType<PlayerInteractor>();
        if (playerInteractor != null)
        {
            playerInteractor.EquipItem(GetComponent<EquippedItem>());
            this.enabled = false;

            InteractedSuccessfully();
        }
        else
        {
            Debug.LogWarning("PlayerInteractor not found in the scene.");
        }
    }
}