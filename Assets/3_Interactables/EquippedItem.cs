using UnityEngine;

public class EquippedItem : MonoBehaviour
{
    private int _usedCount; // Tracks if the item has been used in this game session
    public int usedCount => _usedCount;
    public bool HasBeenUsedThisGame => _usedCount != 0;

    public virtual void UseItem()
    {
        // Default implementation can be empty or provide basic functionality
        Debug.Log("Using item: " + gameObject.name);

        _usedCount++; // Mark as used
    }
}
