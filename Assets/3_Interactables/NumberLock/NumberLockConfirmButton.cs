using UnityEngine;

public class NumberLockConfirmButton : MonoBehaviour
{
    [SerializeField] NumberLock numberLock; // Reference to the NumberLock component
    public void Interact()
    {
        print("Confirm Button Pressed"); // Log when the button is pressed
        numberLock.CheckNumber(); // Check the entered number when the button is pressed
    }
}