using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Interactable))]
public class ButtonInteractAnimation : MonoBehaviour
{
    Animator _animator;

    Interactable _interactable;

    private void Awake()
    {
        _interactable = GetComponent<Interactable>();
        _animator = GetComponent<Animator>();

        if (_interactable == null)
        {
            Debug.LogError("ButtonInteractAnimation requires an Interactable component.");
        }

        if (_animator == null)
        {
            Debug.LogError("ButtonInteractAnimation requires an animator component.");
        }

        _interactable.OnTryInteract += PlayInteractAnimation;
    }

    private void PlayInteractAnimation(bool interactPossible)
    {
        if (_animator == null)
        {
            Debug.LogError("Animator is not assigned for ButtonInteractAnimation.");
        }

        if (interactPossible)
        {
            _animator.SetTrigger("Interact");
        }
        else
        {
            _animator.SetTrigger("CannotInteract");
        }
    }
}
