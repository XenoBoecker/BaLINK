using System;
using UnityEngine;

public class EquippedItem : MonoBehaviour
{
    private int _usedCount; // Tracks if the item has been used in this game session
    public int usedCount => _usedCount;
    public bool HasBeenUsedThisGame => _usedCount != 0;

    protected bool _isEquipped; // Tracks if the item is currently equipped
    public bool IsEquipped => _isEquipped;

    public event Action<bool> OnEquippedChanged;

    public void SetIsEquipped(bool isEquipped)
    {
        if (_isEquipped == isEquipped) return; // No change in state

        _isEquipped = isEquipped;

        OnEquippedChanged?.Invoke(_isEquipped);
    }

    public virtual void UseItem()
    {
        // Default implementation can be empty or provide basic functionality
        Debug.Log("Using item: " + gameObject.name);

        _usedCount++; // Mark as used
    }
}
