using UnityEngine;

public class EquippedItem : MonoBehaviour
{

    public virtual void UseItem()
    {
        // Default implementation can be empty or provide basic functionality
        Debug.Log("Using item: " + gameObject.name);
    }
}
