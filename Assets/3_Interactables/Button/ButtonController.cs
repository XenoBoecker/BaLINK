using UnityEngine;
using UnityEngine.Events;

public class ButtonController : Interactable
{
    [SerializeField] private UnityEvent _onClick;

    protected override void Interact()
    {
        _onClick?.Invoke();
        InteractedSuccessfully();
    }
}
