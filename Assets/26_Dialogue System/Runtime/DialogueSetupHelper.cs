using BetterAttributes;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueSetupHelper : MonoBehaviour
{
    [SerializeField] private DialogueSequence _dialogueSequence;
    [SerializeField] private int _dialogueLineIndex;
    private AudioSource _source;
    
    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && !_source.isPlaying)
        {
            SetupDialogueLine();
        } 
    }

    public void SetupDialogueLine()
    {
        _source.clip = _dialogueSequence.Lines[_dialogueLineIndex].Clip;
        _source.Play();

        int index = 0;
        StartCoroutine(RecordTimestamp(index));
    }

    public IEnumerator RecordTimestamp(int currentSegmentIndex)
    {
        if (currentSegmentIndex == _dialogueSequence.Lines[_dialogueLineIndex].Segments.Length) { yield break; }
        _dialogueSequence.Lines[_dialogueLineIndex].Segments[currentSegmentIndex].SetPercentAudioShow(_source.time / _dialogueSequence.Lines[_dialogueLineIndex].Clip.length);
        yield return new WaitUntil(() => { return Input.GetKeyDown(KeyCode.Space); });
        yield return new WaitUntil(() => { return Input.GetKeyUp(KeyCode.Space); });
        StartCoroutine(RecordTimestamp(currentSegmentIndex + 1));
    }
}
