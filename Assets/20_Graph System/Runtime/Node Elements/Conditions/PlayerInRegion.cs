using UnityEngine;

[NodeElement(NodeType.Condition, NodeType.IfElse)]
[RequireComponent(typeof(PlayerDetectionRegion))]
public class PlayerInRegion : Condition
{
    private PlayerDetectionRegion _detectionRegion;
    [Tooltip("Minimum amount of time the player must be in the region")]
    [SerializeField] float _minimumDuration;

    public override bool ConditionIsMet()
    {
        if (_detectionRegion == null)
        {
            _detectionRegion = ReferencedObject.GetComponent<PlayerDetectionRegion>();
        }

        return _invertCondition != (_detectionRegion.PlayerInRegion && _detectionRegion.PlayerContainedDuration >= _minimumDuration);
    }

    public override string GetName()
    {
        return "Player In Region";
    }
}
