using System;
using UnityEngine;

[NodeElement(NodeType.Condition)]
public class RangeCondition : Condition
{
    [SerializeField] float _range;

    public override bool ConditionIsMet()
    {
        throw new NotImplementedException();
    }

    public override string GetName()
    {
        return "Range Condition";
    }
}
