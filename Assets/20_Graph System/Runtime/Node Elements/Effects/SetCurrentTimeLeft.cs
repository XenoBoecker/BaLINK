using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(Bomb))]
public class SetCurrentTimeLeft : Effect
{
    [SerializeField] private float _remainingTime = 10f;
    public override void TriggerEffect()
    {
        if (ReferencedObject.TryGetComponent(out Bomb bomb))
        {
            bomb.SetCurrentTimeLeft(_remainingTime);
        }
    }

    public override string GetName()
    {
        return "Set Timer Current Time Left";
    }
}
