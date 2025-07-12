using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MenuController
{
    private InputSystem_Actions _inputActions;
    private PlayerFreezeController _playerFreezeController;
    public bool IsPaused => MenuIsVisible;

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _playerFreezeController = FindAnyObjectByType<PlayerFreezeController>();
    }

    void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.Pause.performed += PausePerformed;
    }

    void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.Player.Pause.performed -= PausePerformed;
    }

    void PausePerformed(InputAction.CallbackContext context)
    {
        if (PlayerZoomedInOnObject()) { return; }

        if (!IsPaused)
        {
            MenuManager.Instance.TryShowMenu(this);
        } 
/*        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            MenuManager.Instance.TryHideMenu(this);
        }*/
        SetPauseState();
    }

    public override void ShowMenu()
    {
        base.ShowMenu();
        SetPauseState();
    }

    public override void HideMenu()
    {
        base.HideMenu();
        SetPauseState();
    }

    private bool PlayerZoomedInOnObject()
    {
        return Camera.main == null;
    }

    private void SetPauseState()
    {
        Time.timeScale = IsPaused ? 0.0f : 1.0f;
        _playerFreezeController.SetFreeze(IsPaused);
    }
}
