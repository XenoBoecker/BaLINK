using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(EquippedItem))]

public class DropItem : Effect
{
    public override void TriggerEffect()
    {
        Destroy(ReferencedObject);
    }

    public override string GetName()
    {
        return "Drop Item";
    }
}
