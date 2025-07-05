using GameEvents;
using System;
using TMPro;
using UnityEngine;

public class DoAccuracyTest : IntroSequenceEvent
{
    [SerializeField] private TMP_Text _counterText;
    [SerializeField] private GameObject _isAccurateControls;
    private int _counter = 0;
    private bool _finished = false;

    public override void TriggerSequenceEvent()
    {
        _counter = 0;
        _counterText.text = _counter.ToString();

        InputEvents.onPlayerBlinked += Blinked;
        _isAccurateControls.SetActive(true);
    }

    private void Blinked()
    {
        _counter++;
        _counterText.text = _counter.ToString();
    }

    public void Continue()
    {
        InputEvents.onPlayerBlinked -= Blinked;
        _isAccurateControls.SetActive(false);
        _counterText.text = "";
        _finished = true;
    }

    public void Retry()
    {
        InputEvents.onPlayerBlinked -= Blinked;
        _isAccurateControls.SetActive(false);
        _counterText.text = "";
    }

    public override bool IsFinished()
    {
        return _finished;
    }
}
