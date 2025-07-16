using UnityEngine;

public class BombTimerObserver : MonoBehaviour
{
    Bomb bomb;
    
    private void Awake()
    {
    }

    private void Start()
    {
        bomb = FindAnyObjectByType<Bomb>();
    }

    public void StartBombTimer()
    {
        if (bomb != null)
        {
            bomb.StartBombTimer();
        }
    }

    public void SetTimeLeft(float newTimeLeft)
    {
        if (bomb != null)
        {
            bomb.SetCurrentTimeLeft(newTimeLeft);
        }
    }

    public float GetTimeLeft()
    {
        if (bomb != null)
        {
            return bomb.GetCurrentTimeLeft();
        }
        return 0f;
    }
}
