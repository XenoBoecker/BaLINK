using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(PlayerObserver))]
public class FreezePlayer : Effect
{
    [SerializeField] private bool _targetState;

    public override string GetName()
    {
        return "Freeze Player";
    }

    public override void TriggerEffect()
    {
        Debug.Log($"Setting player freeze state to {_targetState}");
        if (ReferencedObject.TryGetComponent(out PlayerObserver playerObserver))
        {
            playerObserver.SetFreeze(_targetState);
        } 
        else
        {
            throw new System.Exception("Could not find a PlayerObserver component");
        }
    }
}
