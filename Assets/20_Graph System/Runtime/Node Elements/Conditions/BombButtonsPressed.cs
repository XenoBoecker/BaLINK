using UnityEngine;

[NodeElement(NodeType.Condition, NodeType.IfElse)]
public class BombButtonsPressed : Condition
{
    [SerializeField] private ButtonType _buttonType;
    [SerializeField] private int _minimumNumberPressed;
    Bomb _bomb;

    public override bool ConditionIsMet()
    {
        _bomb = FindFirstObjectByType<Bomb>();
        if (_bomb == null)
        {
            Debug.LogWarning("Could not find Bomb component in loaded scene(s)");
            return false;
        }

        int numberOfButtonsPressed = 0;
        switch (_buttonType)
        {
            case ButtonType.Good:
                numberOfButtonsPressed = _bomb.GetNumberOfCorrectButtonsPressed();
                break;
            case ButtonType.Bad:
                numberOfButtonsPressed = _bomb.GetNumberOfWrongButtonsPressed();
                break;
        }

        return _invertCondition != (numberOfButtonsPressed >= _minimumNumberPressed);
    }

    public override string GetName()
    {
        return "Bomb Buttons Pressed";
    }

    private enum ButtonType
    {
        Good,
        Bad
    }
}