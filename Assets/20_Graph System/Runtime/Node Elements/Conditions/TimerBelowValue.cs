using UnityEngine;

[NodeElement(NodeType.Condition, NodeType.IfElse)]
[RequireComponent(typeof(BombTimerObserver))]
public class TimerBelowValue : Condition
{
    [SerializeField] private float _checkValue;

    BombTimerObserver _bombTimerObserver;
    public override bool ConditionIsMet()
    {
        if(_bombTimerObserver == null)
        {
            _bombTimerObserver = ReferencedObject.GetComponent<BombTimerObserver>();
        }

        if(_bombTimerObserver.GetTimeLeft() < _checkValue != _invertCondition)
        {
            return _invertCondition != false;
        }

        return _invertCondition != true;
    }

    public override string GetName()
    {
        return "Timer is below check value";
    }
}
