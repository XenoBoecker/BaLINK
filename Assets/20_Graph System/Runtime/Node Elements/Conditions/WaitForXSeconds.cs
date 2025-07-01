using UnityEngine;

[NodeElement(NodeType.Condition, NodeType.IfElse)]
public class WaitForXSeconds : Condition
{
    [SerializeField] private float _delay;
    float _startTime = -1;

    public override bool ConditionIsMet()
    {
        if (_startTime == -1)
        {
            _startTime = Time.time;
            return false;
        }

        return _invertCondition != (Mathf.Abs(_startTime - Time.time) >= _delay);
    }

    public override string GetName()
    {
        return "Wait For X Seconds";
    }
}
