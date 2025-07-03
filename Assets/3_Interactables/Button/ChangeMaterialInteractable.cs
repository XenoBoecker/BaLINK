using UnityEngine;

public class ChangeMaterialInteractable : Interactable
{
    [SerializeField] private MeshRenderer targetObject;
    [SerializeField] private Material newMaterial;

    [SerializeField] bool switchBackAndForth = false;

    private Material oldMaterial;

    protected override void Interact()
    {
        InteractedSuccessfully();

        if(switchBackAndForth && oldMaterial != null)
        {
            targetObject.material = oldMaterial;
            oldMaterial = null;
            return;
        }
        oldMaterial = targetObject.material;
        targetObject.material = newMaterial;
    }
}
