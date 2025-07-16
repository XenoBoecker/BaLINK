using GameEvents;
using System.Collections;
using TMPro;
using UnityEngine;

public class ShowTextSequenceEvent : IntroSequenceEvent
{
    [TextArea(2,4)]
    [SerializeField] private string[] _textsToShow;
    [SerializeField] private bool _overrideTextColor;
    [SerializeField] private Color _textColor;
    [SerializeField] private TMP_Text _textObject;
    [SerializeField] private float _textRevealSpeed;
    [SerializeField] private float _delayBeforeFade;
    [SerializeField] private float _textFadeSpeed;
    [SerializeField] private float _delayBetweenLines;
    [SerializeField] private float _delayBeforeEffectEnd;
    [SerializeField] private bool _fadeOutText = true;
    [SerializeField] private AudioSystemClip[] _typingSounds;
    bool _sequenceOver = false;
    int _textIndex = 0;
    Color _originalTextColor;
    bool _isShowingText = false;

    public override void TriggerSequenceEvent()
    {
        _textIndex = 0;
        _originalTextColor = _textObject.color;
        _sequenceOver = false;

        if (_overrideTextColor)
        {
            _originalTextColor = _textColor;
        }
        if (_originalTextColor.a < 0.001f)
        {
            _originalTextColor = new Color(_originalTextColor.r, _originalTextColor.g, _originalTextColor.b, 1);
        }

        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        if(_isShowingText)
        {
            Debug.LogError("ShowTextSequenceEvent is already showing text. Skipping this call. This should not happen I think. Sometimes the Coroutine is started multiple times, causing it to increase the _textIndex to a too high value", this);
            yield break;
        }

        _isShowingText = true;

        string text = "";
        _textObject.color = _originalTextColor;

        int counter = 0;

        foreach (char c in _textsToShow[_textIndex])
        {
            text += c;
            SetText(text);

            if (counter % 2 == 0 && !char.IsWhiteSpace(c) && _typingSounds != null && _typingSounds.Length != 0)
            {
                ObjectEvents.PlayAudio(_typingSounds[Random.Range(0, _typingSounds.Length)], Vector3.zero);
            }

            counter++;
            yield return new WaitForSecondsRealtime(1f / _textRevealSpeed);
        }

        yield return new WaitForSecondsRealtime(_delayBeforeFade);

        if (_fadeOutText)
        {
            for (float i = 0; i < _textFadeSpeed; i+= Time.unscaledDeltaTime)
            {
                _textObject.color = new Color(_originalTextColor.r, _originalTextColor.g, _originalTextColor.b, 1-(i / _textFadeSpeed));
                yield return null;
            }
            _textObject.color = new Color(_originalTextColor.r, _originalTextColor.g, _originalTextColor.b, 0);
        }

        _textIndex++;

        if (_textIndex == _textsToShow.Length)
        {
            yield return new WaitForSecondsRealtime(_delayBeforeEffectEnd);
            _sequenceOver = true;
            _isShowingText = false;
            yield break;
        }

        yield return new WaitForSecondsRealtime(_delayBetweenLines);

        _isShowingText = false;

        StartCoroutine(ShowText());
    }

    private void SetText(string text)
    {
        _textObject.text = text;
    }

    public override bool IsFinished()
    {
        return _sequenceOver;
    }
}