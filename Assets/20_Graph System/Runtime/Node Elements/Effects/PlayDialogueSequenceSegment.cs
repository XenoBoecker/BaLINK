using UnityEngine;

[NodeElement(NodeType.Effect)]
public class PlayDialogueSequenceSegment : Effect
{
    [SerializeField] private DialogueSequence _sequence;
    [SerializeField] private int _firstLineIndex = -1;
    [SerializeField] private int _lastLineIndex = -1;
    private DialogueRunner _runner;

    public override string GetName()
    {
        return "Play Sequence Segment";
    }

    public override void TriggerEffect()
    {
        GameObject foundObject = GameObject.FindGameObjectWithTag("Dialogue Runner");
        if (foundObject == null || !foundObject.TryGetComponent(out _runner))
        {
            throw new System.Exception("There is no object tagged 'Dialogue Runner' in the scene");
        }

        int startingIndex = _firstLineIndex == -1 ? 0 : _firstLineIndex;
        int lastLineIndex = _lastLineIndex == -1 ? _sequence.Lines.Length - 1 : _lastLineIndex;

        _runner.PlayDialogueSequenceSegment(_sequence, startingIndex, lastLineIndex);
    }
}
