using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(Bomb))]
public class StartBombTimer : Effect
{
    public override void TriggerEffect()
    {
        if (ReferencedObject.TryGetComponent(out Bomb bomb))
        {
            bomb.StartBombTimer();
        }
    }

    public override string GetName()
    {
        return "Start the Bomb Timer";
    }
}

