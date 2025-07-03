using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(Interactable))]
public class SetInteractableEnabled : Effect
{
    [SerializeField] private bool _enabled = true;
    public override void TriggerEffect()
    {
        if (ReferencedObject.TryGetComponent(out Interactable interactable))
        {
            interactable.SetInteractionEnabled(_enabled);
        }
    }

    public override string GetName()
    {
        return "Set Interactable Enabled";
    }
}
