using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EyeDataCollector))]
[RequireComponent(typeof(BlinkDetection))]
public class EyeCalibration : MonoBehaviour
{
    private bool _calibrationEnabled;
    private bool _eyesOpenCalibrated;
    private bool _eyesClosedCalibrated;
    private bool _finishedCalibrating;

    private EyeDataCollector _eyeDataCollector;
    private BlinkDetection _eyeBlinkDetector;

    List<EyeData> _openedData;
    List<EyeData> _closedData;

    public event Action OnFailedToCalibrate;
    public event Action OnStaredCalibratingEyePosition;

    public bool FinishedCalibrating => _finishedCalibrating;

    private void Awake()
    {
        _eyeDataCollector = GetComponent<EyeDataCollector>();
        _eyeBlinkDetector = GetComponent<BlinkDetection>();
    }

    public void EnableCalibration()
    {
        _calibrationEnabled = true;
        _openedData = new List<EyeData>();
        _closedData = new List<EyeData>();
    }

    internal void DisableCalibration()
    {
        _calibrationEnabled = false;
    }

    private void Update()
    {
        if (!_calibrationEnabled) { return; }

        if (!_eyesOpenCalibrated)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Eyes Opened Calibration Started");
                OnStaredCalibratingEyePosition?.Invoke();
            }

            _openedData.Add(_eyeDataCollector.GetEyeData());
        
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _eyesOpenCalibrated = true;
            }
        }
        else if (!_eyesClosedCalibrated )
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnStaredCalibratingEyePosition?.Invoke();
                Debug.Log("Eyes Closed Calibration Started");
            }

            _closedData.Add(_eyeDataCollector.GetEyeData());

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _eyesClosedCalibrated = true;
            }
        } 
        else if (_eyesOpenCalibrated && _eyesClosedCalibrated && !_finishedCalibrating)
        {
            _finishedCalibrating = true;

            float closedAverageValue = 0;
            float openedAverageValue = 0;

            for (int i = 0; i < _closedData.Count; i++)
            {
                if (_closedData[i].EyeAspectRatioLeft.Equals(float.NaN) || _closedData[i].EyeAspectRatioRight.Equals(float.NaN)) { continue; }
                closedAverageValue += (_closedData[i].EyeAspectRatioLeft + _closedData[i].EyeAspectRatioRight) * 0.5f;
            }
            closedAverageValue /= (float)_closedData.Count;

            for (int i = 0; i < _openedData.Count; i++)
            {
                if (_openedData[i].EyeAspectRatioLeft.Equals(float.NaN) || _openedData[i].EyeAspectRatioRight.Equals(float.NaN)) { continue; }
                openedAverageValue += (_openedData[i].EyeAspectRatioLeft + _openedData[i].EyeAspectRatioRight) * 0.5f;
            }
            openedAverageValue /= (float)_openedData.Count;

            Debug.Log(_closedData.Count);
            Debug.Log(closedAverageValue);

            float difference = openedAverageValue - closedAverageValue;

            if (difference < 0)
            {
                _eyesOpenCalibrated = false;
                _eyesClosedCalibrated = false;
                _finishedCalibrating = false;
                Debug.LogWarning("Could Not Calibrate. Try Again");
                OnFailedToCalibrate?.Invoke();
            }

            _eyeBlinkDetector.SetThresholds(openedAverageValue - difference * 0.45f, closedAverageValue + difference * 0.45f);
        }
    }

    internal void ResetCalibrator()
    {
        _eyesOpenCalibrated = false;
        _eyesClosedCalibrated = false;
        _finishedCalibrating = false;
    }
}
