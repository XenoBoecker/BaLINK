using UnityEngine;

public class Door : MonoBehaviour
{
    Animator _animator;

    bool _isOpen;
    public bool IsOpen => _isOpen; // Expose the open state of the door

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator component not found on the Door object.");
        }
    }

    public void OpenDoor()
    {
        _isOpen = true; // Set the door state to open
        _animator.SetTrigger("Open");
    }

    public void DoorIsLockedAnimation()
    {
        _animator.SetTrigger("Locked");
    }

    public void CloseDoor()
    {
        _isOpen = false; // Set the door state to closed
        _animator.SetTrigger("Close");
    }

    public void ToggleDoor()
    {
        if (_isOpen)
        {
            CloseDoor();
        } 
        else
        {
            OpenDoor();
        }
    }
}
