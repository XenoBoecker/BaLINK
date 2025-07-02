using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(Target))]
public class ToggleTargetSpeed : Effect
{
    public override string GetName()
    {
        return "Toggle Target Speed";
    }

    public override void TriggerEffect()
    {
        if (ReferencedObject.TryGetComponent(out Target target))
        {
            target.ToggleMovementSpeed();
        }
    }
}
