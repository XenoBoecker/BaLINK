using UnityEngine;

[NodeElement(NodeType.Effect)]
public class DebugString : Effect
{
    [SerializeField] private string _text;

    public override string GetName()
    {
        return "Debug String";
    }

    public override void TriggerEffect()
    {
        Debug.Log(_text);
    }
}
