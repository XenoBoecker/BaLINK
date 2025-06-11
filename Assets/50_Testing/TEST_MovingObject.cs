using UnityEngine;
using GameEvents;

[RequireComponent(typeof(Custom.Animator))]
public class TEST_MovingObject : MonoBehaviour
{
    Custom.Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Custom.Animator>();
    }

    private void OnEnable()
    {
        InputEvents.onPlayerBlinked += PlayerBlinked;
    }

    private void OnDisable()
    {
        InputEvents.onPlayerBlinked -= PlayerBlinked;
    }

    private void PlayerBlinked()
    {
        Debug.Log("Blink");
        _animator.NextFrame();
    }
}
