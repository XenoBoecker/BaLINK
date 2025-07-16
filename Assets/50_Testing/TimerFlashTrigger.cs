using GameEvents;
using UnityEngine;

public class TimerFlashTrigger : MonoBehaviour
{
    public void DoTimerFlash()
    {
        ObjectEvents.WrongButtonPressed();
    }
}
