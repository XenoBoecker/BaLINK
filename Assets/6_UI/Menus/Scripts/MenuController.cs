using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    private InputSystem_Actions _inputActions;
    private MenuPanel[] _menuPanels;
    private MenuPanel _selectedPanel;
    private bool _menuIsVisible;

    [SerializeField] private MenuType _menuType;
    [SerializeField] private GameObject _menuContents;
    [SerializeField] private MenuController[] _blockerMenus;

    [SerializeField] private UnityEvent _onMenuShown;
    [SerializeField] private UnityEvent _onMenuHidden;


    public bool MenuIsVisible => _menuIsVisible;
    public MenuController[] BlockerMenus => _blockerMenus;
    public MenuType MenuType => _menuType;

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

    public virtual void ShowMenu()
    {
        if (_menuContents != null && _menuContents.activeSelf) { return; }

        _menuContents.SetActive(true);
        _menuIsVisible = true;
        _onMenuShown?.Invoke();
    }

    public virtual void HideMenu()
    {
        if (_menuContents != null && !_menuContents.activeSelf) { return; }

        _menuContents.SetActive(false);
        _menuIsVisible = false;
        _onMenuHidden?.Invoke();
    }

    private void GetAllMenuPanels()
    {
        _menuPanels = GetComponentsInChildren<MenuPanel>(true);
    }

    private void OnCanceled(InputAction.CallbackContext context)
    {
        if (_selectedPanel != null && _selectedPanel.EscapePanel != null)
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