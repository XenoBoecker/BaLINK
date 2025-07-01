using System;
using UnityEngine;

public class NumberLock : MonoBehaviour
{
    [SerializeField] Door door; // Reference to the door to open
    [SerializeField] string correctNumber = "123"; // Example correct number

    [SerializeField] NumberSelector[] numberSelectors;

    public event Action OnNumberLockOpened; // Event to notify when the lock is opened

    public void CheckNumber()
    {
        if (numberSelectors.Length == 0)
        {
            Debug.LogError("No NumberSelectors assigned to the NumberLock.");
            return;
        }
        string enteredNumber = "";
        for (int i = 0; i < numberSelectors.Length; i++)
        {
            enteredNumber += numberSelectors[i].CurrentNumber.ToString();
        }
        if (enteredNumber == correctNumber)
        {
            Debug.Log("Correct number entered: " + enteredNumber);

            OnNumberLockOpened?.Invoke();

            OpenDoor();
        }
        else
        {
            Debug.Log("Incorrect number entered: " + enteredNumber);
            // Optionally, trigger a failure response
            WrongCodeEntered();
        }
    }

    void OpenDoor()
    {
        door.OpenDoor();
    }

    void WrongCodeEntered()
    {
        door.DoorIsLockedAnimation();
    }
}
