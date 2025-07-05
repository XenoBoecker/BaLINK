using System;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private float _delayAfterPlayingLine;
    [SerializeField] private DialogueLineTextSegment[] _segments;

    public AudioClip Clip => _clip;
    public float DelayAfterPlayingLine => _delayAfterPlayingLine;
    public DialogueLineTextSegment[] Segments => _segments;

    internal void SetDelay(float delay)
    {
        _delayAfterPlayingLine = delay;
    }
}
