using UnityEngine;

[NodeElement(NodeType.Condition)]
[RequireComponent(typeof(Interactable))]
public class PlayerHasInteractedWithObject : Condition
{
    private Interactable _interactable;

    public override bool ConditionIsMet()
    {
        if (_interactable == null)
        {
            _interactable = ReferencedObject.GetComponent<Interactable>();
        }

        return _invertCondition != _interactable.HasBeenInteractedWithThisGame;
    }

    public override string GetName()
    {
        return "Player Has Interacted With Object";
    }
}
