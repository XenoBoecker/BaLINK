using GameEvents;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    private PlayerInteractor _playerInteractor;
    [SerializeField] private Image _cursorNormalImageComponent;
    [SerializeField] private Image _cursorInteractionImageComponent;

    bool _cursorEnabled;

    private void Awake()
    {
        _playerInteractor = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerInteractor>();
    }

    private void OnEnable()
    {
        ObjectEvents.OnDisableCursor += DisableCursor;
        ObjectEvents.OnEnableCursor += EnableCursor;
    }

    private void EnableCursor()
    {
        _cursorEnabled = true;
        _cursorNormalImageComponent.enabled = true;
        _cursorInteractionImageComponent.enabled = true;
    }

    private void DisableCursor()
    {
        _cursorEnabled = false;
        _cursorNormalImageComponent.enabled = false;
        _cursorInteractionImageComponent.enabled = false;
    }

    private void Update()
    {
        if (!_cursorEnabled) { return; }
        _cursorInteractionImageComponent.enabled = _playerInteractor.IsMouseHoverOverInteractable();
    }
}
