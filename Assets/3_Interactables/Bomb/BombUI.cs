using TMPro;
using UnityEngine;

public class BombUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    Bomb bomb;

    private void Awake()
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

    void Start()
    {
        if (bomb == null)
        {
            Debug.LogError("Bomb reference is not set in BombUI.");
            return;
        }
    }

    void LateUpdate()
    {
        float timeLeft = bomb.GetCurrentTimeLeft();

        // timer text in minutes and seconds
        if (timeLeft <= 0)
        {
            timerText.text = "0.00"; // Display 0.00 when time is up
            return;
        }
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);

        timerText.text = $"{minutes:00}:{seconds:00}"; // Format as MM:SS.MS
    }

    public void SetBomb(Bomb newBomb)
    {
        bomb = newBomb;
    }
}