using UnityEngine;

[NodeElement(NodeType.Effect)]
public class FreezePlayer : Effect
{
    [SerializeField] private bool _targetState;

    public override string GetName()
    {
        return "Freeze Player";
    }

    public override void TriggerEffect()
    {
        if (Player.TryGetComponent(out PlayerFreezeController controller))
        {
            controller.SetFreeze(_targetState);
        } 
        else
        {
            throw new System.Exception("Could not find a FreezePlayerController component attached to the player");
        }
    }
}
