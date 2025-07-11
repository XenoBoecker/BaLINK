using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LerpMaterialColorSequenceEvent : IntroSequenceEvent
{
    [SerializeField] private float _colorLerpSpeed;
    [SerializeField] private Image _image;
    [SerializeField] private Material _targetMaterial;
    [SerializeField] private string[] _materialColorFields;
    private Material _initialMaterial;
    bool _sequenceOver = false;
    private Material _material;

    public override void TriggerSequenceEvent()
    {
        _initialMaterial = _image.material;
        _material = new Material(_image.material);
        _image.material = _material;

        StartCoroutine(LerpColors());
        _sequenceOver = false;
    }

    private IEnumerator LerpColors()
    {
        for (float i = 0; i < _colorLerpSpeed; i += Time.deltaTime)
        {
            foreach (string fieldName in _materialColorFields)
            {
                _material.SetColor(fieldName, Color.Lerp(_initialMaterial.GetColor(fieldName), _targetMaterial.GetColor(fieldName), i / _colorLerpSpeed));
            }
            yield return null;
        }

        foreach (string fieldName in _materialColorFields)
        {
            _material.SetColor(fieldName, _targetMaterial.GetColor(fieldName));
        }
        _sequenceOver = true;
    }

    public override bool IsFinished()
    {
        return _sequenceOver;
    }
}
