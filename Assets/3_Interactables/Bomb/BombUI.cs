using TMPro;
using UnityEngine;

public class BombUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    Bomb bomb;

    bool timerNegative;

    void Start()
    {
        bomb = FindAnyObjectByType<Bomb>();

        if (timerText == null)
        {
            Debug.LogError("Timer text reference is not set in BombUI.");
        }

        if (bomb == null)
        {
            Debug.LogError("Bomb is not found.", this);
        }
    }

    void LateUpdate()
    {
        float timeLeft = bomb.GetCurrentTimeLeft();

        // timer text in minutes and seconds
        if (timeLeft <= 0)
        {
            timerNegative = true;
            timeLeft = -timeLeft;
        }
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);

        if (timerNegative)
        {
            timerText.text = $"-{minutes:00}:{seconds:00}";
        }
        else
        {
            timerText.text = $"{minutes:00}:{seconds:00}"; // Format as MM:SS.MS
        }
    }

    public void SetBomb(Bomb newBomb)
    {
        bomb = newBomb;
    }
}
