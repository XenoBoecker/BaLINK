using UnityEngine;

public abstract class Condition : NodeElement
{
    [SerializeField] protected bool _invertCondition;
    public override void TriggerEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExitNode() { }
}
