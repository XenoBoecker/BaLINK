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
        timerText.text = timeLeft > 0 ? timeLeft.ToString("F2") : "0.00";
    }

    public void SetBomb(Bomb newBomb)
    {
        bomb = newBomb;
    }
}