using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(EquippedItem))]
public class DropItem : Effect
{
    public override void TriggerEffect()
    {
        if (ReferencedObject.TryGetComponent(out EquippedItem item))
        {
            item.SetIsEquipped(false);
        }
        Destroy(ReferencedObject);
    }
    
    public override string GetName()
    {
        return "Drop Item";
    }
}
