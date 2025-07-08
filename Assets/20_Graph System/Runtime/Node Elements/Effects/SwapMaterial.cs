using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(WireGenerator))]
public class SwapMaterial : Effect
{
    [SerializeField]
    private Material _material;

    public override string GetName()
    {
        return "Swap Material";
    }

    public override void TriggerEffect()
    {
        if (ReferencedObject.TryGetComponent(out WireGenerator generator))
        {
            generator.Material = _material;
        }
    }
}
