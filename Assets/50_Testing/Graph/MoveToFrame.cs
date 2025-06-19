using System;
using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(Custom.Animator))]
public class MoveToFrame : Effect
{
    [SerializeField] int _targetFrame;
    public float TargetFrame => _targetFrame;

    public override void TriggerEffect()
    {
        if (ReferencedObject.TryGetComponent(out Custom.Animator animator))
        {
            animator.SetAnimationStateToFrame(_targetFrame);
        }
    }

    public override string GetName()
    {
        return "Move to Frame";

    }
}
