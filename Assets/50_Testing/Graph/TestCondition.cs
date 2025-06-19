using System;
using UnityEngine;

[NodeElement(NodeType.Condition)]
public class TestCondition : NodeElement
{
    [SerializeField] bool _value;

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
        return "Test Condition";
    }
}
