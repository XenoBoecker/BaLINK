using System;
using UnityEngine;

[NodeElement(NodeType.Condition)]
public class RangeCondition : NodeElement
{
    [SerializeField] float _range;

    public override void Initialize(GameObject referencedObject)
    {
        base.Initialize(referencedObject);
    }
    
    public override bool ConditionIsMet()
    {
        throw new NotImplementedException();
    }

    public override string GetName()
    {
        return "Range Condition";
    }
}
