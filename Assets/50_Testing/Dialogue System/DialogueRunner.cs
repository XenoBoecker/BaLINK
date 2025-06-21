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
            ShowDialogueText(line.Text);
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

    private void ShowDialogueText(string text)
    {
        _text.text = text;
    }
}
