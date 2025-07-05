using UnityEngine;

public class ChangeMaterialInteractable : Interactable
{
    [SerializeField] private MeshRenderer targetObject;
    [SerializeField] private Material newMaterial;

    private Material oldMaterial;

    protected override void Interact()
    {
        InteractedSuccessfully();

        if(_canBeInteractedWithMultipleTimes && _interactedCount % 2 == 0)
        {
            targetObject.material = oldMaterial;
            return;
        }
        oldMaterial = targetObject.material;
        targetObject.material = newMaterial;
    }
}
