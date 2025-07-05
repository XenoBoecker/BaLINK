using Unity.VisualScripting;
using UnityEngine;

[NodeElement(NodeType.Condition)]
public class DialogueNotPlaying : Condition
{
    private DialogueRunner _runner;

    public override bool ConditionIsMet()
    {
        GameObject foundObject = GameObject.FindGameObjectWithTag("Dialogue Runner");
        if (foundObject == null || !foundObject.TryGetComponent(out _runner))
        {
            throw new System.Exception("There is no object tagged 'Dialogue Runner' in the scene");
        }

        return _invertCondition != !_runner.IsPlaying;
    }

    public override string GetName()
    {
        return "Dialogue Not Playing";
    }
}
