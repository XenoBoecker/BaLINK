using UnityEngine;

public struct QueuedDialogue
{
    [SerializeField] private DialogueSequence _sequence;
    [SerializeField] private int _firstIndex;
    [SerializeField] private int _lastIndex;

    public DialogueSequence Sequence => _sequence;
    public int FirstIndex => _firstIndex;
    public int LastIndex => _lastIndex;

    public QueuedDialogue(DialogueSequence sequence, int firstIndex, int lastIndex)
    {
        _sequence = sequence;
        _firstIndex = firstIndex;
        _lastIndex = lastIndex;
    }
}
