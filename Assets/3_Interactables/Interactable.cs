using UnityEngine;

public class Interactable : MonoBehaviour
{
    private bool _hasBeenInteractedWithThisGame = false; // Tracks if the object has been interacted with in this game session
    public bool HasBeenInteractedWithThisGame => _hasBeenInteractedWithThisGame;

    public virtual void Interact()
    {
        // Default interaction logic can be overridden by derived classes
        Debug.Log("Interacted with " + gameObject.name);

        _hasBeenInteractedWithThisGame = true; // Mark as interacted with
    }
}
