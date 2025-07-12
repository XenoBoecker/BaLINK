using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    private MenuController[] _menus;
    private List<MenuController> _activeMenus;

    private void Awake()
    {
        if (Instance != null)
        {
            throw new System.Exception("Too many MenuManagers in scene!");
        }
        Instance = this;
    }

    private void Start()
    {
        _menus = FindObjectsByType<MenuController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
    }
    
    internal void SetMenuState(bool targetState, MenuType targetMenu, MenuShowMode mode)
    {
        if (TryGetMenuFromMenuType(targetMenu, out MenuController menu))
        {
            if (targetState)
            {
                TryShowMenu(menu, mode);
            }
            else
            {
                TryHideMenu(menu);
            }
        }
    }

    private bool TryGetMenuFromMenuType(MenuType targetMenu, out MenuController foundMenu)
    {
        foreach (MenuController menu in _menus)
        {
            if (menu.MenuType == targetMenu)
            {
                foundMenu = menu;
                return true;
            }
        }

        foundMenu = null;
        return false;
    }

    internal void TryShowMenu(MenuController menu, MenuShowMode mode = MenuShowMode.Exclusive)
    {
        if (!MenuCanBeShown(menu)) { return; }

        ShowMenu(menu, mode);
    }

    internal void TryHideMenu(MenuController menu)
    {
        HideMenu(menu);
    }

    private void ShowMenu(MenuController menu, MenuShowMode mode)
    {
        if (mode == MenuShowMode.Exclusive)
        {
            _activeMenus.Clear();
        }
        menu.ShowMenu();
        _activeMenus.Add(menu);

        HideAllNonActiveMenus();
    }

    private void HideMenu(MenuController menu)
    {
        _activeMenus.Remove(menu);
        menu.HideMenu();
    }

    private bool MenuCanBeShown(MenuController menu)
    {
        if (_activeMenus == null)
        {
            _activeMenus = new List<MenuController>();
            return true;
        }

        foreach (MenuController activeMenu in _activeMenus)
        {
            if (menu.BlockerMenus.Contains(activeMenu))
            {
                return false;
            }
        }

        return true;
    }

    private void HideAllNonActiveMenus()
    {
        foreach (MenuController menu in _menus)
        {
            if (_activeMenus.Contains(menu)) { continue; }
            menu.HideMenu();
        }
    }

}