using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    Animator animator;

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
        animator.SetTrigger("Open");
    }

    public void DoorIsLockedAnimation()
    {
        animator.SetTrigger("Locked");
    }

    public void CloseDoor()
    {
        animator.SetTrigger("Close");
    }
}
