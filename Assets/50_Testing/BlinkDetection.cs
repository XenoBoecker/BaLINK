using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Tasks.Vision.FaceLandmarker;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EyeDataCollector))]
public class BlinkDetection : MonoBehaviour
{
    [SerializeField] float _eyesClosedThreshold = 0.2f;
    [SerializeField] float _eyesOpenedThreshold = 0.35f;
    
    private bool _eyesClosed = false;

    private EyeDataCollector _eyeDataCollector;

    private void Awake()
    {
        _eyeDataCollector = GetComponent<EyeDataCollector>();
    }

    void Update()
    {
        EyeData eyeData = _eyeDataCollector.GetEyeData();

        float eyeLeft = eyeData.EyeAspectRatioLeft;
        float eyeRight = eyeData.EyeAspectRatioRight;

        if (eyeLeft == float.NaN || eyeRight == float.NaN) { return; }

        if (_eyesClosed)
        {
            if (eyeLeft > _eyesOpenedThreshold && eyeRight > _eyesOpenedThreshold)
            {
                _eyesClosed = false;
            }
        } 
        else
        {
            if (eyeLeft < _eyesClosedThreshold && eyeRight < _eyesClosedThreshold)
            {
                _eyesClosed = true;
                Debug.Log("Blinked");
            }
        }
    }

    public void SetThresholds(float openThreshold, float closedThreshold)
    {
        _eyesOpenedThreshold = openThreshold;
        _eyesClosedThreshold = closedThreshold;
    }
}
