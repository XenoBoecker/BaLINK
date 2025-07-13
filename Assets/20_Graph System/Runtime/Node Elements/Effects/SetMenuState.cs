using UnityEngine;

[NodeElement(NodeType.Effect)]
public class SetMenuState : Effect
{
    [SerializeField] private bool _targetState;
    [SerializeField] private MenuType _targetMenu;
    [SerializeField] private MenuShowMode _showMode;

    public override string GetName()
    {
        return "Show Menu";
    }

    public override void TriggerEffect()
    {
        if (MenuManager.Instance == null) { return; }
        MenuManager.Instance.SetMenuState(_targetState, _targetMenu, _showMode);
    }
}
