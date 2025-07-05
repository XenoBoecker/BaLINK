using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(CrossbowItem))]
public class TakeAwayEquippedItem : Effect
{
    public override void TriggerEffect()
    {
        if (ReferencedObject.TryGetComponent(out EquippedItem item))
        {
            item.RemoveCurrentryEquippedItem();
        }
    }

    public override string GetName()
    {
        return "Remove currently equipped item";
    }
}
