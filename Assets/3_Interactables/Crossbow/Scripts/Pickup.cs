using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EquippedItem))]
public class Pickup : Interactable
{
    [SerializeField] private UnityEvent OnPickedUp;

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
            OnPickedUp?.Invoke();
        }
        else
        {
            Debug.LogWarning("PlayerInteractor not found in the scene.");
        }
    }
}