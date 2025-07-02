using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(PlayerInteractor))]
public class TakeAwayEquippedItem : Effect
{
    public override void TriggerEffect()
    {
        if (ReferencedObject.TryGetComponent(out PlayerInteractor playerInteractor))
        {
            playerInteractor.TakeAwayEquippedItem();
        }
    }

    public override string GetName()
    {
        return "Take away equipped item";
    }
}
