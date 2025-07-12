using System;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class CursorCanvas : MonoBehaviour
{
    [SerializeField] private GameObject cursorPanel;
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

        if (playerInteractor == null)
        {
            Debug.LogError("PlayerInteractor not found in the scene. Please ensure it is present.", this);
            return;
        }

        if(pauseMenu == null)
        {
            Debug.LogError("PauseMenu not found in the scene. Please ensure it is present.", this);
            return;
        }
    }

    private void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (GameHasCrashed())
        {
            Cursor.lockState = CursorLockMode.None;
            movingCursor.SetCursor(FollowMouseCursor.CursorType.Crash);
        }
        else if (pauseMenu.IsPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            movingCursor.SetCursor(FollowMouseCursor.CursorType.Pause);
            cursorPanel.SetActive(false);
            return;
        }
        else if (ItemEquipped())
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            movingCursor.SetCursor(FollowMouseCursor.CursorType.None);
            cursorPanel.SetActive(false);
            return;
        }
        else if (PlayerIsInMinigame())
        {
            Cursor.lockState = CursorLockMode.None;
            movingCursor.SetCursor(FollowMouseCursor.CursorType.NumberLock);
            cursorPanel.SetActive(false);
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        movingCursor.SetCursor(FollowMouseCursor.CursorType.None);
        cursorPanel.SetActive(true);

        if(normalSprite == null || canInteractSprite == null)
        {
            Debug.LogWarning("Cursor sprites are not assigned in the CursorCanvas script.");
            return;
        }

        if (playerInteractor.IsMouseHoverOverInteractable())
        {
            cursorPanel.GetComponent<UnityEngine.UI.Image>().sprite = canInteractSprite;
        }
        else
        {
            cursorPanel.GetComponent<UnityEngine.UI.Image>().sprite = normalSprite;
        }
    }

    private bool ItemEquipped()
    {
        return playerInteractor.IsItemEquipped;
    }

    private bool PlayerIsInMinigame()
    {
        return Camera.main == null || !Camera.main.enabled;
    }

    private bool GameHasCrashed()
    {
        if (crashScreen == null) return false;
        return crashScreen.HasCrashed;
    }
}
