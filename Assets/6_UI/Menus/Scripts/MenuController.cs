using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    private InputSystem_Actions _inputActions;
    private MenuPanel[] _menuPanels;
    private MenuPanel _selectedPanel;

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        GetAllMenuPanels();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.Cancel.performed += OnCanceled;
    }

    private void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.Player.Cancel.performed -= OnCanceled;
    }

    private void GetAllMenuPanels()
    {
        _menuPanels = GetComponentsInChildren<MenuPanel>(true);
    }

    private void OnCanceled(InputAction.CallbackContext context)
    {
        if (_selectedPanel.EscapePanel != null)
        {
            ShowPanel(_selectedPanel.EscapePanel);
        }
    }
    
    private void HideAllPanels()
    {
        foreach (MenuPanel panel in _menuPanels)
        {
            panel.Hide();
        }
    }

    public void ShowPanel(MenuPanel panel)
    {
        HideAllPanels();

        _selectedPanel = panel;
        _selectedPanel.Show();
    }
}