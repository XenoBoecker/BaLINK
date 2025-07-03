using System;
using System.Threading.Tasks;
using UnityEngine;

public class IntroSequenceHandler : MonoBehaviour
{
    [SerializeField] IntroSequenceEvent[] _sequenceEvents;

    private void Start()
    {
        StartSequence();
    }

    private async void StartSequence()
    {
        for (int i = 0; i < _sequenceEvents.Length; i++)
        {
            await DoSequenceEvent(_sequenceEvents[i]);
        }
    }

    private async Task DoSequenceEvent(IntroSequenceEvent introSequenceEvent)
    {
        introSequenceEvent.TriggerSequenceEvent();
        while (!introSequenceEvent.IsFinished())
        {
            await Task.Yield();
        }
    }
}
