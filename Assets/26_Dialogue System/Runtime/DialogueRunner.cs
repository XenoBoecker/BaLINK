using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueRunner : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private AudioSource _source;

    private bool _isPlaying;
    public bool IsPlaying => _isPlaying;

    private List<QueuedDialogue> _queuedDialogues = new List<QueuedDialogue>();

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    internal void PlayDialogueSequenceSegment(DialogueSequence sequence, int firstIndex, int lastIndex)
    {
        if (IsPlaying)
        {
            //_queuedDialogues.Add(new QueuedDialogue(sequence, firstIndex, lastIndex));
            return;
        }
        StartCoroutine(PlaySequenceSegment(sequence, firstIndex, lastIndex));
    }

    private void PlayDialogueLine(DialogueSequence sequence, int dialogueLineIndex)
    {
        if (sequence.TryGetDialogueLine(dialogueLineIndex, out DialogueLine line))
        {
            if (line.Segments == null || line.Segments.Length == 0)
            {
                throw new Exception($"Dialogue line number {dialogueLineIndex} of {sequence.name} does not have any text defined");
            }

            ShowDialogueText(line, 0);
            StartCoroutine(ClearDialogueText(line.Clip.length));
            _source.clip = line.Clip;
            _source.volume *= SaveSystem.Data.DialogueVolume;
            _source.Play();
        }
    }

    IEnumerator ClearDialogueText(float delay)
    {
        yield return new WaitForSeconds(delay);
        _text.text = "";
    }

    private void ShowDialogueText(DialogueLine line, int index)
    {
        _text.text = line.Segments[index].Text;
        StartCoroutine(WaitForNextDialogueSegment(line, index));
    }

    IEnumerator WaitForNextDialogueSegment(DialogueLine line, int currentIndex)
    {
        if (currentIndex + 1 >= line.Segments.Length) { yield break; }
        yield return new WaitUntil(() => { return (_source.time/line.Clip.length >= line.Segments[currentIndex + 1].PercentAlongAudioToShow); });
        ShowDialogueText(line, currentIndex + 1);
    }

    private IEnumerator PlaySequenceSegment(DialogueSequence sequence, int firstIndex, int lastIndex)
    {
        _isPlaying = true;
        for (int i = firstIndex; i < lastIndex + 1; i++)
        {
            PlayDialogueLine(sequence, i);
            yield return new WaitUntil(() => { return !_source.isPlaying; });
            if (i != lastIndex)
            {
                yield return new WaitForSeconds(sequence.Lines[i].DelayAfterPlayingLine);
            }
        }

        if (_queuedDialogues.Count > 0)
        {
            StartCoroutine(PlaySequenceSegment(_queuedDialogues[0].Sequence, _queuedDialogues[0].FirstIndex, _queuedDialogues[0].LastIndex));
            _queuedDialogues.RemoveAt(0);
            yield break;
        }

        _isPlaying = false;
    }
}
