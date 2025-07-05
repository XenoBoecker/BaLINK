using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected bool _interactionEnabled = true;
    [SerializeField] protected bool _canBeInteractedWithMultipleTimes = false;

    protected int _interactedCount; // Tracks if the object has been interacted with in this game session
    public int InteractedCount => _interactedCount;
    public bool HasBeenInteractedWithThisGame => _interactedCount > 0;

    bool _canBeInteractedWith => _interactionEnabled && !(!_canBeInteractedWithMultipleTimes && HasBeenInteractedWithThisGame);

    public event Action OnInteracted;
    public event Action<bool> OnTryInteract;

    protected virtual void Interact()
    {

    }

    public bool TryInteract()
    {
        OnTryInteract?.Invoke(_canBeInteractedWith);

        if (!_canBeInteractedWith)
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
