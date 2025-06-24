using UnityEngine;

public class DoorButton : Interactable
{
    [SerializeField] private GameObject door; // Reference to the door GameObject

    [SerializeField] private bool onlyInteractableOnce;

    public override void Interact()
    {
        base.Interact();

        if (door != null)
        {
            if(onlyInteractableOnce && HasBeenInteractedWithThisGame)
            {
                return; // Exit if the button can only be interacted with once
            }

            // Toggle the door's active state when the button is pressed
            door.SetActive(!door.activeSelf);
        }
        else
        {
            Debug.LogWarning("Door reference is not set in the DoorButton script.");
        }
    }
}
