using UnityEngine;

[NodeElement(NodeType.Effect)]
public class PlayDialogueSequence : Effect
{
    [SerializeField] private DialogueSequence _sequence;

    private DialogueRunner _runner;

    public override string GetName()
    {
        return "Play Sequence";
    }

    public override void TriggerEffect()
    {
        GameObject foundObject = GameObject.FindGameObjectWithTag("Dialogue Runner");
        if (foundObject == null || !foundObject.TryGetComponent(out _runner))
        {
            throw new System.Exception("There is no object tagged 'Dialogue Runner' in the scene");
        }

        _runner = ReferencedObject.GetComponent<DialogueRunner>();
        _runner.PlayDialogueSequence(_sequence);
    }
}
