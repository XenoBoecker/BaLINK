using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(BombTimerObserver))]
public class StartBombTimer : Effect
{
    public override void TriggerEffect()
    {
        if (ReferencedObject.TryGetComponent(out BombTimerObserver bomb))
        {
            bomb.StartBombTimer();
        }
    }

    public override string GetName()
    {
        return "Start the Bomb Timer";
    }
}

