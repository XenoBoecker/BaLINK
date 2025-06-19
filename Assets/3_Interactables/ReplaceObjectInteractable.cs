using UnityEngine;

public class ReplaceObjectInteractable : Interactable
{
    [SerializeField] private GameObject startObject;
    [SerializeField] private GameObject objectAfterInteraction; // The object to replace when interacted with
    public override void Interact()
    {
        base.Interact();
        if (startObject != null && objectAfterInteraction != null)
        {
            startObject.SetActive(false); // Deactivate the original object
            objectAfterInteraction.SetActive(true); // Activate the new object
        }
        else
        {
            Debug.LogWarning("Object to replace is not set in the ReplaceObjectInteractable script.");
        }
    }
}