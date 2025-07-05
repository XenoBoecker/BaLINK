using System;
using UnityEngine;

[System.Serializable]
public class DialogueLineTextSegment
{
    [TextArea(1, 6)]
    [SerializeField] private string _text;
    [SerializeField, Range(0, 1)] private float _percentAlongAudioToShow;

    public string Text => _text;
    public float PercentAlongAudioToShow => _percentAlongAudioToShow;

    internal void SetPercentAudioShow(float value)
    {
        _percentAlongAudioToShow = value;
    }
}
