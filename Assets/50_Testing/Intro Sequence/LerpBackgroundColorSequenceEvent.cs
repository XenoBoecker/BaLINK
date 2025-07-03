using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LerpBackgroundColorSequenceEvent : IntroSequenceEvent
{
    [SerializeField] private float _colorLerpSpeed;
    [SerializeField] private Color _targetColor;
    [SerializeField] private Image _backgroundImage;
    private Color _initialColor;
    bool _sequenceOver = false;

    public override void TriggerSequenceEvent()
    {
        _initialColor = _backgroundImage.color;
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        for (float i = 0; i < _colorLerpSpeed; i += Time.deltaTime)
        {
            _backgroundImage.color = Color.Lerp(_initialColor, _targetColor, i/_colorLerpSpeed);
            yield return null;
        }
        _backgroundImage.color = _targetColor;
        _sequenceOver = true;
    }

    public override bool IsFinished()
    {
        return _sequenceOver;
    }
}
