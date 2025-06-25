using UnityEngine;

public class ReplaceObjectInteractable : Interactable
{
    [SerializeField] private GameObject startObject;
    [SerializeField] private GameObject objectAfterInteraction; // The object to replace when interacted with

    private void Start()
    {
        // Ensure the initial state is set correctly
        if (startObject != null && objectAfterInteraction != null)
        {
            startObject.SetActive(true); // Ensure the original object is active at the start
            objectAfterInteraction.SetActive(false); // Ensure the new object is inactive at the start
        }
        else
        {
            Debug.LogWarning("Start or after interaction objects are not set in the ReplaceObjectInteractable script.");
        }
    }
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