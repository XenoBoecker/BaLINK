using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected bool _interactionEnabled = true;

    protected int _interactedCount; // Tracks if the object has been interacted with in this game session
    public int interactedCount => _interactedCount;
    public bool HasBeenInteractedWithThisGame => _interactedCount > 0;

    public event Action OnInteracted;

    protected virtual void Interact()
    {

    }

    public bool TryInteract()
    {
        if (!_interactionEnabled)
        {
            return false;
        }
        else
        {
            Interact();
            return true;
        }
    }

    protected void InteractedSuccessfully()
    {
        _interactedCount++;

        OnInteracted?.Invoke();
    }
    public void SetInteractionEnabled(bool enable)
    {
        _interactionEnabled = enable;
    }
}
