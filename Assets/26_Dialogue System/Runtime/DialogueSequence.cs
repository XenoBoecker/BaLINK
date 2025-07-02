using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Sequence", menuName = "Dialogue Sequence", order = -1000)]
public class DialogueSequence : ScriptableObject
{
    [SerializeField] DialogueLine[] _lines;

    public DialogueLine[] Lines => _lines;

    internal bool TryGetDialogueLine(int dialogueLineIndex, out DialogueLine line)
    {
        line = null;
        if (dialogueLineIndex < 0 || dialogueLineIndex >= _lines.Length) { return false; }
        line = _lines[dialogueLineIndex];
        return line != null;
    }
}