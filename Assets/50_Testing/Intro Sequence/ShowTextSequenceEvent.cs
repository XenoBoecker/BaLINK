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
    bool _sequenceOver = false;
    int _textIndex = 0;
    Color _originalTextColor;

    public override void TriggerSequenceEvent()
    {
        _textIndex = 0;
        _originalTextColor = _textObject.color;
        if (_overrideTextColor)
        {
            _originalTextColor = _textColor;
        }
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        string text = "";
        _textObject.color = _originalTextColor;

        foreach (char c in _textsToShow[_textIndex])
        {
            text += c;
            SetText(text);
            yield return new WaitForSeconds(1f / _textRevealSpeed);
        }

        yield return new WaitForSeconds(_delayBeforeFade);

        for (float i = 0; i < _textFadeSpeed; i+= Time.deltaTime)
        {
            _textObject.color = new Color(_originalTextColor.r, _originalTextColor.g, _originalTextColor.b, 1-(i / _textFadeSpeed));
            yield return null;
        }
        _textObject.color = new Color(_originalTextColor.r, _originalTextColor.g, _originalTextColor.b, 0);

        _textIndex++;
        if (_textIndex == _textsToShow.Length)
        {
            yield return new WaitForSeconds(_delayBeforeEffectEnd);
            _sequenceOver = true;
            yield break;
        }

        yield return new WaitForSeconds(_delayBetweenLines);

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