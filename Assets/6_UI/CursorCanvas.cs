using System;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class CursorCanvas : MonoBehaviour
{
    [SerializeField] private GameObject cursorPanel;

    [SerializeField] private Sprite normalSprite, canInteractSprite;

    [SerializeField] FollowMouseCursor movingCursor;

    PlayerInteractor playerInteractor;
    PauseMenu pauseMenu;

    private void Start()
    {
        playerInteractor = FindAnyObjectByType<PlayerInteractor>();
        pauseMenu = FindAnyObjectByType<PauseMenu>();

        if(playerInteractor == null)
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
        if (ItemEquipped())
        {
            Cursor.lockState = CursorLockMode.Locked;
            cursorPanel.SetActive(false);
            return;
        }else if (pauseMenu.IsPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            movingCursor.SetCursor(FollowMouseCursor.CursorType.Pause);
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
        else if (GameHasCrashed())
        {
            Cursor.lockState = CursorLockMode.None;
            movingCursor.SetCursor(FollowMouseCursor.CursorType.Crash);
        }

        Cursor.lockState = CursorLockMode.Locked;
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
        // Replace with actual logic to determine if the game has crashed
        return false; // Placeholder for demonstration purposes
    }
}
