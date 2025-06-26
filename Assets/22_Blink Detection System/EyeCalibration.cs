using UnityEngine;

[RequireComponent(typeof(EyeDataCollector))]
[RequireComponent(typeof(BlinkDetection))]
public class EyeCalibration : MonoBehaviour
{
    private bool _eyesOpenCalibrated;
    private bool _eyesClosedCalibrated;
    private bool _finishedCalibrating;

    private EyeDataCollector _eyeDataCollector;
    private BlinkDetection _eyeBlinkDetector;

    EyeData _openedData;
    EyeData _closedData;

    private void Awake()
    {
        _eyeDataCollector = GetComponent<EyeDataCollector>();
        _eyeBlinkDetector = GetComponent<BlinkDetection>();
    }

    private void Update()
    {
        if (!_eyesOpenCalibrated && Input.GetKeyDown(KeyCode.Space))
        {
            _eyesOpenCalibrated = true;
            _openedData = _eyeDataCollector.GetEyeData();
            Debug.Log("Eyes Opened Calibrated: " + _openedData.EyeAspectRatioLeft + ", " + _openedData.EyeAspectRatioRight);
        } 
        else if (!_eyesClosedCalibrated && Input.GetKeyDown(KeyCode.Space))
        {
            _eyesClosedCalibrated = true;
            _closedData = _eyeDataCollector.GetEyeData();
            Debug.Log("Eyes Closed Calibrated: " + _closedData.EyeAspectRatioLeft + ", " + _closedData.EyeAspectRatioRight);
        } 
        else if (_eyesOpenCalibrated && _eyesClosedCalibrated && !_finishedCalibrating)
        {
            _finishedCalibrating = true;

            float closedAverageValue = (_closedData.EyeAspectRatioLeft + _closedData.EyeAspectRatioRight) * 0.5f;
            float openedAverageValue = (_openedData.EyeAspectRatioLeft + _openedData.EyeAspectRatioRight) * 0.5f;

            float difference = openedAverageValue - closedAverageValue;

            if (difference < 0)
            {
                _eyesOpenCalibrated = false;
                _eyesClosedCalibrated = false;
                _finishedCalibrating = false;
                Debug.LogWarning("Could Not Calibrate. Try Again");
            }

            _eyeBlinkDetector.SetThresholds(openedAverageValue - difference * 0.45f, closedAverageValue + difference * 0.45f);
        }
    }
}
