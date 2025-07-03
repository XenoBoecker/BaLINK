using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class IntroSequenceHandler : MonoBehaviour
{
    [SerializeField] IntroSequenceEvent[] _sequenceEvents;
    Task _currentSequence;

    private void Start()
    {
        StartCoroutine(StartSequence());
    }

    public void JumpToSequence(IntroSequenceEvent sequence)
    {
        StopAllCoroutines();
        for (int i = 0; i < _sequenceEvents.Length; i++)
        {
            if (_sequenceEvents[i].Equals(sequence))
            {
                StartCoroutine(StartSequence(i));
                return;
            }
        }

        Debug.LogWarning("Could not find the referenced sequence to jump to in the sequence array");
    }

    private IEnumerator StartSequence(int startingIndex = 0)
    {
        Debug.Log(_sequenceEvents[startingIndex].name);
        for (int i = startingIndex; i < _sequenceEvents.Length; i++)
        {
            _sequenceEvents[i].TriggerSequenceEvent();
            yield return new WaitUntil(() => { return _sequenceEvents[i].IsFinished(); });
        }
    }
}
