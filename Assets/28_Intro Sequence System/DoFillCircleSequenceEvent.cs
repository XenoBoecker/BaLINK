using System;
using UnityEngine;
using UnityEngine.UI;

public class DoFillCircleSequenceEvent : IntroSequenceEvent
{
    [SerializeField] private GameObject _circleElementHolder;
    [SerializeField] private Image _circle;
    [SerializeField] private float _fillDuration;

    bool _fillingCircle = false;
    bool _isFinished = false;

    public override void TriggerSequenceEvent()
    {
        _isFinished = false;
        _circle.fillAmount = 0f;
        _circleElementHolder.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isFinished)
        {
            _fillingCircle = true;
        }

        if (!_fillingCircle) { return; }
        
        _circle.fillAmount += (1f/_fillDuration) * Time.deltaTime;
        if (_circle.fillAmount >= 1f)
        {
            Finish();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _fillingCircle = false;
            Finish();
        }
    }

    private void Finish()
    {
        _isFinished = true;
        _circle.fillAmount = 1f;
        _circleElementHolder.SetActive(false);
    }

    public override bool IsFinished()
    {
        return _isFinished;
    }
}
