using UnityEngine;

public class Interactable : MonoBehaviour
{
    private int _interactedCount; // Tracks if the object has been interacted with in this game session
    public int interactedCount => _interactedCount;
    public bool HasBeenInteractedWithThisGame => _interactedCount != 0;

    public virtual void Interact()
    {
        // Default interaction logic can be overridden by derived classes
        Debug.Log("Interacted with " + gameObject.name);

        _interactedCount++; // Mark as interacted with
    }
}
