using UnityEngine;

public class NumberSelectorInteractable : MonoBehaviour
{
    [SerializeField] NumberSelector numberSelector; // Reference to the NumberSelector component
    [SerializeField] bool isIncrementing = true; // Whether this selector increments or decrements the number
    public void Interact()
    {
        if(isIncrementing)
        {
            numberSelector.IncrementNumber();
        }
        else
        {
            numberSelector.DecrementNumber();
        }
    }
}
