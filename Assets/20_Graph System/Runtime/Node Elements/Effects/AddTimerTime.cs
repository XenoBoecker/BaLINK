using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(Bomb))]
public class AddTimerTime : Effect
{
    [SerializeField] private float _timeToAdd = 10f;
    public override void TriggerEffect()
    {
        if (ReferencedObject.TryGetComponent(out Bomb bomb))
        {
            bomb.AddTime(_timeToAdd);
        }
    }

    public override string GetName()
    {
        return "Set Timer Current Time Left";
    }
}
