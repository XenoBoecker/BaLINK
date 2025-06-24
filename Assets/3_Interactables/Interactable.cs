using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected int _interactedCount; // Tracks if the object has been interacted with in this game session
    public int interactedCount => _interactedCount;
    public bool HasBeenInteractedWithThisGame => _interactedCount > 0;

    public event Action OnInteracted;

    public virtual void Interact()
    {

    }

    protected void InteractedSuccessfully()
    {
        _interactedCount++;

        OnInteracted?.Invoke();
    }
}
