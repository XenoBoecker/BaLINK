using UnityEngine;

public abstract class Effect : NodeElement
{
    public override bool ConditionIsMet()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExitNode() {}
}
