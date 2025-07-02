using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(TargetManager))]
public class ToggleTargetsSpeed : Effect
{
    public override void TriggerEffect()
    {
        if(ReferencedObject.TryGetComponent(out TargetManager targetManager))
        {
            targetManager.ToggleTargetMoveSpeed();
        }
    }

    public override string GetName()
    {
        return "Toggle Targets Movement Speed";
    }
}
