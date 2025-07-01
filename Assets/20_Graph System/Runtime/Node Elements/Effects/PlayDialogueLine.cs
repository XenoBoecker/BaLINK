using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(DialogueRunner))]
public class PlayDialogueLine : Effect
{
    [SerializeField] private DialogueSequence _sequence;
    [SerializeField] private int _lineIndex;

    private DialogueRunner _runner;

    public override void TriggerEffect()
    {
        _runner = ReferencedObject.GetComponent<DialogueRunner>();
        _runner.PlayDialogueLine(_sequence, _lineIndex);
    }

    public override string GetName()
    {
        return "Play Dialogue Line";
    }
}