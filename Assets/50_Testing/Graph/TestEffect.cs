using UnityEngine;

[NodeElement(NodeType.Effect)]
public class TestEffect : NodeElement
{
    public override bool ConditionIsMet()
    {
        return true;
    }

    public override string GetName()
    {
        return "Test Effect";
    }
}
