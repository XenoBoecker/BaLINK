using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueRunner : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayDialogueLine(DialogueSequence sequence, int dialogueLineIndex)
    {
        if (sequence.TryGetDialogueLine(dialogueLineIndex, out DialogueLine line))
        {
            ShowDialogueText(line, 0);
            StartCoroutine(ClearDialogueText(line.Clip.length));
            _source.clip = line.Clip;
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
}
