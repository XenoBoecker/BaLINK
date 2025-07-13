using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MenuController
{
    private InputSystem_Actions _inputActions;
    private PlayerFreezeController _playerFreezeController;
    public bool IsPaused => MenuIsVisible;

    protected override void Awake()
    {
        base.Awake();
        _inputActions = new InputSystem_Actions();
        _playerFreezeController = FindAnyObjectByType<PlayerFreezeController>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _inputActions.Enable();
        _inputActions.Player.Pause.performed += PausePerformed;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
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
