using System;
using UnityEngine;

public class CursorCanvas : MonoBehaviour
{
    [SerializeField] private GameObject cursorPanel;

    [SerializeField] private Sprite normalSprite, canInteractSprite;

    PlayerInteractor playerInteractor;

    private void Start()
    {
        playerInteractor = FindAnyObjectByType<PlayerInteractor>();
    }

    private void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (ItemEquipped())
        {
            cursorPanel.SetActive(false);
            return;
        }else if (PlayerIsInMinigame())
        {
            cursorPanel.SetActive(false);
            return;
        }

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
}