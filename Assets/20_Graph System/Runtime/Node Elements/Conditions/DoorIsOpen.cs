using UnityEngine;

[NodeElement(NodeType.Condition, NodeType.IfElse)]
[RequireComponent(typeof(DoorController))]
public class DoorIsOpen : Condition
{
    private DoorController _door;

    public override bool ConditionIsMet()
    {
        if (_door == null)
        {
            _door = ReferencedObject.GetComponent<DoorController>();
        }

        return _invertCondition != _door.IsOpen;
    }

    public override string GetName()
    {
        return "Door Is Open";
    }
}
