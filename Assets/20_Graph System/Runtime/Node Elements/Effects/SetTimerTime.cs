using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(BombTimerObserver))]
public class SetTimerTime : Effect
{
    [SerializeField] private float _newTimeLeft = 10f;
    public override void TriggerEffect()
    {
        if (ReferencedObject.TryGetComponent(out BombTimerObserver bomb))
        {
            bomb.SetTimeLeft(_newTimeLeft);
        }
    }

    public override string GetName()
    {
        return "Set Timer Current Time Left";
    }
}

