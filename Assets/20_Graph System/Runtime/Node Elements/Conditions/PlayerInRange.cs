using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[NodeElement(NodeType.Condition)]
public class PlayerInRange : Condition
{
    [SerializeField] private float _distance;

    public override bool ConditionIsMet()
    {
        float currentDistance = Vector3.Distance(ReferencedObject.transform.position, Player.transform.position);
        return _invertCondition != (currentDistance < _distance);
    }

    public override string GetName()
    {
        return "Player In Range";
    }
}
