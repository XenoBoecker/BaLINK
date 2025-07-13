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

        // Debug.Log($"Checking if timer is below {_checkValue} with current value: {_bombTimerObserver.GetTimeLeft()}");

        if (_bombTimerObserver.GetTimeLeft() < _checkValue != _invertCondition)
        {
            Debug.Log($"Timer is below {_checkValue}: {_bombTimerObserver.GetTimeLeft()}");
            return true;
        }

        return false;
    }

    public override string GetName()
    {
        return "Timer is below check value";
    }
}
