using UnityEngine;

[NodeElement(NodeType.Effect)]
public class ToggleObject : Effect
{
    [SerializeField] bool _targetState;

    public override void TriggerEffect()
    {
        ReferencedObject.SetActive(_targetState);
    }
    
    public override string GetName()
    {
        return "Toggle Object";
    }
}
