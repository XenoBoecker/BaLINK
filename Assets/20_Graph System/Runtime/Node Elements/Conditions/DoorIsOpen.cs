using UnityEngine;

[NodeElement(NodeType.Condition, NodeType.IfElse)]
[RequireComponent(typeof(Door))]
public class DoorIsOpen : Condition
{
    private Door _door;

    public override bool ConditionIsMet()
    {
        if (_door == null)
        {
            _door = ReferencedObject.GetComponent<Door>();
        }

        return _invertCondition != _door.IsOpen;
    }

    public override string GetName()
    {
        return "Door Is Open";
    }
}
