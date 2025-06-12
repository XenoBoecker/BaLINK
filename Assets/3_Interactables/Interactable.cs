using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interact()
    {
        // Default interaction logic can be overridden by derived classes
        Debug.Log("Interacted with " + gameObject.name);
    }
}
