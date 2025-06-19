using UnityEngine;

[NodeElement(NodeType.Effect)]
public class TestEffect : Effect
{

    public override void TriggerEffect()
    {
        throw new System.NotImplementedException();
    }

    public override string GetName()
    {
        return "Test Effect";
    }
}
