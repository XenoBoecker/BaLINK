using System;
using UnityEngine;

public class TestCondition : Condition
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

    public override string GetConditionName()
    {
        return "Test Condition";
    }
}
