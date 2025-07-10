using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Interactable))]
public class ButtonInteractAnimation : MonoBehaviour
{
    [SerializeField] private bool _unpressImmediate;
    
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

    }

    private void OnEnable()
    {
        _interactable.OnTryInteract += PlayTryInteractAnimation;
        _interactable.OnInteracted += PlayInteractedAnimation;
    }


    private void OnDisable()
    {
        _interactable.OnTryInteract -= PlayTryInteractAnimation;
        _interactable.OnInteracted -= PlayInteractedAnimation;
    }

    private void PlayInteractedAnimation()
    {
        if (_animator == null)
        {
            Debug.LogError("Animator is not assigned for ButtonInteractAnimation.");
        }

        _animator.SetTrigger("Interact");
        if (_unpressImmediate)
        {
            _animator.SetTrigger("Interact Secondary");
        }
    }
    
    private void PlayTryInteractAnimation(bool interactPossible)
    {
        if (_animator == null)
        {
            Debug.LogError("Animator is not assigned for ButtonInteractAnimation.");
        }

        if (interactPossible)
        {
            return;
        }

        _animator.SetTrigger("CannotInteract");
    }
}
