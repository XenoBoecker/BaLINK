using System;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class CursorCanvas : MonoBehaviour
{
    [SerializeField] private Image fixedCursorImage, movingCursorImage;
    [SerializeField] private Texture2D _defaultCursor;
    [SerializeField] private Sprite normalSprite, canInteractSprite;

    [SerializeField] FollowMouseCursor movingCursor;

    PlayerInteractor playerInteractor;
    PauseMenuController pauseMenu;
    CrashScreen crashScreen;

    private void Start()
    {
        Cursor.SetCursor(_defaultCursor, new Vector2(8, 8), CursorMode.ForceSoftware);

        playerInteractor = FindAnyObjectByType<PlayerInteractor>();
        pauseMenu = FindAnyObjectByType<PauseMenuController>();
        crashScreen = FindAnyObjectByType<CrashScreen>();
    }

    private void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        Cursor.visible = false;

        if (GameHasCrashed())
        {
            SetMovingCursor(FollowMouseCursor.CursorType.Crash);
            return;
        }
        else if (pauseMenu != null && pauseMenu.IsPaused)
        {
            SetMovingCursor(FollowMouseCursor.CursorType.Pause);
            return;
        }
        else if (ItemEquipped())
        {
            SetMovingCursor(FollowMouseCursor.CursorType.None);
            fixedCursorImage.gameObject.SetActive(false);
            return;
        }
        else if (PlayerIsInMinigame())
        {
            SetMovingCursor(FollowMouseCursor.CursorType.NumberLock);
            return;
        }

        if(normalSprite == null || canInteractSprite == null)
        {
            Debug.LogWarning("Cursor sprites are not assigned in the CursorCanvas script.");
            return;
        }

        if (playerInteractor == null) // player in main menu
        {
            SetMovingCursor(FollowMouseCursor.CursorType.Pause);

            ShowHoverInteractUI(IsMouseOverUIButton());
        }
        else
        {
            SetMovingCursor(FollowMouseCursor.CursorType.None);

            ShowHoverInteractUI(playerInteractor.IsMouseHoverOverInteractable());
        }

    }

    void SetMovingCursor(FollowMouseCursor.CursorType cursorType)
    {
        if (movingCursor == null)
        {
            Debug.LogError("Moving cursor is not assigned in the CursorCanvas script.");
            return;
        }

        if(cursorType == FollowMouseCursor.CursorType.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            movingCursorImage.gameObject.SetActive(false);
            fixedCursorImage.gameObject.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            movingCursorImage.gameObject.SetActive(true);
            fixedCursorImage.gameObject.SetActive(false);
        }

        movingCursor.SetCursor(cursorType);
    }

    void ShowHoverInteractUI(bool v)
    {
        if (v)
        {
            fixedCursorImage.sprite = canInteractSprite;
            movingCursorImage.sprite = canInteractSprite;
        }
        else
        {
            fixedCursorImage.sprite = normalSprite;
            movingCursorImage.sprite = normalSprite;
        }
    }

    private bool ItemEquipped()
    {
        if (playerInteractor == null) return false;
        return playerInteractor.IsItemEquipped;
    }

    private bool PlayerIsInMinigame()
    {
        return Camera.main == null || !Camera.main.enabled;
    }

    bool IsMouseOverUIButton()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() || UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1);

    }

    private bool GameHasCrashed()
    {
        if (crashScreen == null) return false;
        return crashScreen.HasCrashed;
    }
}
