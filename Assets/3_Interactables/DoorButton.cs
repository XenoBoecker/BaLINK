using UnityEngine;

public class DoorButton : Interactable
{
    [SerializeField] private GameObject door;

    [SerializeField] private bool onlyInteractableOnce;

    protected override void Interact()
    {
        if (door != null)
        {
            if(onlyInteractableOnce && HasBeenInteractedWithThisGame)
            {
                return;
            }

            door.SetActive(!door.activeSelf);

            InteractedSuccessfully();
        }
        else
        {
            Debug.LogWarning("Door reference is not set in the DoorButton script.");
        }
    }
}
