using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(Custom.Animator))]
public class MoveToNextFrame : Effect
{
    public override void TriggerEffect()
    {
        if (ReferencedObject.TryGetComponent(out Custom.Animator animator))
        {
            animator.NextFrame();
        }
    }

    public override string GetName()
    {
        return "Move to Next Frame";
    }
}
