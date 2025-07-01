using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private DialogueLineTextSegment[] _segments;

    public DialogueLineTextSegment[] Segments => _segments;
    public AudioClip Clip => _clip;
}
