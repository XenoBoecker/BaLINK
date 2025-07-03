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
            startObject.SetActive(true);
            objectAfterInteraction.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Start or after interaction objects are not set in the ReplaceObjectInteractable script.");
        }
    }
    protected override void Interact()
    {
        if (startObject != null && objectAfterInteraction != null)
        {
            startObject.SetActive(false);
            objectAfterInteraction.SetActive(true);

            InteractedSuccessfully();
        }
        else
        {
            Debug.LogWarning("Object to replace is not set in the ReplaceObjectInteractable script.");
        }
    }
}