using System;
using UnityEngine;

public class TestCondition2 : Condition
{
    [SerializeField] float _floatValue;

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
