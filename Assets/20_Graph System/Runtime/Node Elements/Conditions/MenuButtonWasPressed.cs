using UnityEngine;

[NodeElement(NodeType.Condition, NodeType.IfElse)]
[RequireComponent(typeof(PauseMenu))]
public class MenuButtonWasPressed : Condition
{
    private PauseMenu _pauseMenu;

    public override bool ConditionIsMet()
    {
        if (_pauseMenu == null)
        {
            _pauseMenu = ReferencedObject.GetComponent<PauseMenu>();
        }

        return _invertCondition != _pauseMenu.MenuButtonWasPressed;
    }

    public override string GetName()
    {
        return "Menu Button Was Pressed";
    }
}
