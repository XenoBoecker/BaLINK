using UnityEngine;

public class CrashScreen : MonoBehaviour
{
    [SerializeField] private GameObject crashScreen;
    [SerializeField] private GameObject notRespondingWindow;

    bool hasCrashed;

    void Start()
    {
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
        Time.timeScale = 0f;
        hasCrashed = true;
        crashScreen.SetActive(true);
    }

    public void HideCrashScreen()
    {
        Time.timeScale = 1f;
        hasCrashed = false;
        crashScreen.SetActive(false);
        notRespondingWindow.SetActive(false);
    }
}
