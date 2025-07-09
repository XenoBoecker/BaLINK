using System;
using System.Collections;
using UnityEngine;

public class DoCalibrationSequenceEvent : IntroSequenceEvent
{
    [SerializeField] private EyeCalibration _calibrator;
    [SerializeField] private IntroSequenceEvent _onFailedCalibrationEvent;
    [SerializeField] private IntroSequenceEvent _circleFillSequence;
    [SerializeField] private IntroSequenceEvent _eyesClosedTextSequence;

    public override void TriggerSequenceEvent()
    {
        _calibrator.ResetCalibrator();
        _calibrator.EnableCalibration();
        _calibrator.OnFailedToCalibrate += Failed;

        StartCoroutine(DoContainedEvents());
    }

    private IEnumerator DoContainedEvents()
    {
        _circleFillSequence.TriggerSequenceEvent();
        yield return new WaitUntil(() => { return _circleFillSequence.IsFinished(); });
        _eyesClosedTextSequence.TriggerSequenceEvent();
        yield return new WaitUntil(() => { return _eyesClosedTextSequence.IsFinished(); });
    }

    private void Failed()
    {
        _calibrator.OnFailedToCalibrate -= Failed;
        _onFailedCalibrationEvent.TriggerSequenceEvent();
        _calibrator.DisableCalibration();
        StartCoroutine(RetryCalibration());
    }

    private IEnumerator RetryCalibration()
    {
        yield return new WaitUntil(() => { return _onFailedCalibrationEvent.IsFinished(); });
        TriggerSequenceEvent();
    }

    public override bool IsFinished()
    {
        return _calibrator.FinishedCalibrating;
    }
}
