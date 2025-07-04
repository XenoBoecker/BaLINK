using UnityEngine;

public class CursorCanvas : MonoBehaviour
{
    [SerializeField] private GameObject cursorPanel;

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