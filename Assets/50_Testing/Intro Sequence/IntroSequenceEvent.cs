using UnityEngine;

public abstract class IntroSequenceEvent : MonoBehaviour
{
    public abstract void TriggerSequenceEvent();
    public abstract bool IsFinished();
}
