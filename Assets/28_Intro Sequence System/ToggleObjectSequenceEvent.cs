using UnityEngine;

public class ToggleObjectSequenceEvent : IntroSequenceEvent
{
    [SerializeField] private GameObject _object;
    [SerializeField] private bool _targetState;

    public override bool IsFinished()
    {
        return true;
    }

    public override void TriggerSequenceEvent()
    {
        _object.SetActive(_targetState);
    }
}
