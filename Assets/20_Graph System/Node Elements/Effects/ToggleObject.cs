using UnityEngine;

[NodeElement(NodeType.Effect)]
public class ToggleObject : Effect
{
    [SerializeField] private bool _activeState;

    public override void TriggerEffect()
    {
        ReferencedObject.SetActive(_activeState);
    }

    public override string GetName()
    {
        return "Toggle Object";
    }
}
