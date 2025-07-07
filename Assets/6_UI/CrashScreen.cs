using UnityEngine;

public class CrashScreen : MonoBehaviour
{
    [SerializeField] private GameObject crashScreen;
    [SerializeField] private GameObject notRespondingWindow;

    PlayerFreezeController _playerFreezeController;

    bool hasCrashed;
    public bool HasCrashed => hasCrashed;

    void Start()
    {
        _playerFreezeController = FindAnyObjectByType<PlayerFreezeController>();

        crashScreen.SetActive(false);
        notRespondingWindow.SetActive(false);
        hasCrashed = false;
    }

    private void Update()
    {
        if (hasCrashed)
        {
            if (Input.GetMouseButtonDown(0))
            {
                notRespondingWindow.SetActive(true);
            }
        }
    }

    public void ShowCrashScreen()
    {
        _playerFreezeController.SetFreeze(true);
        hasCrashed = true;
        crashScreen.SetActive(true);
    }

    public void HideCrashScreen()
    {
        _playerFreezeController.SetFreeze(false);
        hasCrashed = false;
        crashScreen.SetActive(false);
        notRespondingWindow.SetActive(false);
    }
}
