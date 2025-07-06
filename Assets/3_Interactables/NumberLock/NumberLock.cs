using System;
using UnityEngine;

public class NumberLock : MonoBehaviour
{
    [SerializeField] Door door; // Reference to the door to open
    [SerializeField] string correctNumber = "123"; // Example correct number

    [SerializeField] NumberSelector[] numberSelectors;
    bool _isLocked = true; // Indicates if the lock is currently locked

    public event Action OnNumberLockOpened; // Event to notify when the lock is opened

    private void OnEnable()
    {
        for (int i = 0; i < numberSelectors.Length; i++)
        {
            numberSelectors[i].OnNumberChanged += CheckNumber;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < numberSelectors.Length; i++)
        {
            numberSelectors[i].OnNumberChanged -= CheckNumber;
        }
    }

    public void CheckNumber()
    {
        if(!_isLocked)
        {
            Debug.Log("NumberLock is already opened, no need to check again.");
            return;
        }

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
            _isLocked = false;
            OnNumberLockOpened?.Invoke();

            OpenDoor();
        }
        else
        {
            Debug.Log($"Entered number: {enteredNumber} does not match the correct number: {correctNumber}");
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
