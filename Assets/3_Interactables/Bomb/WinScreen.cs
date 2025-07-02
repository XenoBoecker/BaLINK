using UnityEngine;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;

    Bomb bomb;


    private void Awake()
    {
        bomb = FindAnyObjectByType<Bomb>();
        bomb.OnBombDefused += ShowWinScreen;

        winScreen.SetActive(false);
    }

    void ShowWinScreen()
    {
        winScreen.SetActive(true);
    }
}
