using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    Animator animator;

    bool isOpen;
    public bool IsOpen => isOpen; // Expose the open state of the door

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the Door object.");
        }
    }

    public void OpenDoor()
    {
        isOpen = true; // Set the door state to open
        animator.SetTrigger("Open");
    }

    public void DoorIsLockedAnimation()
    {
        animator.SetTrigger("Locked");
    }

    public void CloseDoor()
    {
        isOpen = false; // Set the door state to closed
        animator.SetTrigger("Close");
    }
}
