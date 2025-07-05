using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class NumberSelector : MonoBehaviour
{
    [SerializeField] private GameObject _lockElement;
    [SerializeField] private int _lockElementSides;
    [SerializeField] private float _rotationSpeed;

    public event Action OnNumberChanged;
    private int _currentNumber = 0;
    float _angle = 0;
    bool _numberChanged;
    
    public int CurrentNumber => _currentNumber < 0 ? ((_currentNumber + (_lockElementSides * Mathf.CeilToInt((float)Mathf.Abs(_currentNumber) / _lockElementSides))) % _lockElementSides) : (_currentNumber % _lockElementSides);

    private void Start()
    {
        _lockElement.transform.rotation = Quaternion.identity;
    }

    public void IncrementNumber()
    {
        _currentNumber++;
        _numberChanged = true;
    }


    public void DecrementNumber()
    {
        _currentNumber--;
        _numberChanged = true;
    }

    private void Update()
    {
        float targetRotation = 360f * ((float)_currentNumber / _lockElementSides);

        _angle = Mathf.Lerp(_angle, targetRotation, _rotationSpeed * Time.deltaTime);
        _lockElement.transform.localEulerAngles = new Vector3(_angle, 0, 0);

        if (_numberChanged && Mathf.Abs(targetRotation - _angle) < 1)
        {
            OnNumberChanged?.Invoke();
        }
    }
}
