using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TimeField : MonoBehaviour
{
    [SerializeField] float _slowSpeed;
    float _currentTimeScale => GetCurrentTimeScale();
    List<ITimeAffected> _timeObjectsInRadius = new List<ITimeAffected>();
    bool _timeSlowActive;

    public void SetTimeScaleState(bool timeSlowActive)
    {
        Debug.Log("set time scale");
        _timeSlowActive = timeSlowActive;
        UpdateTimeScale(_currentTimeScale);
    }

    void UpdateTimeScale(float timeScale)
    {
        Debug.Log(timeScale);
        Debug.Log(_timeObjectsInRadius.Count);
        foreach (ITimeAffected timeObject in _timeObjectsInRadius)
        {
            timeObject.SetTimeScale(timeScale);
        }
    }

    private float GetCurrentTimeScale()
    {
        return _timeSlowActive ? _slowSpeed : 1.0f;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out ITimeAffected timeObject))
        {
            timeObject.SetTimeScale(_currentTimeScale);
            AddObject(timeObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent(out ITimeAffected timeObject))
        {
            timeObject.SetTimeScale(1);
            RemoveObject(timeObject);
        }
    }
   
    void AddObject(ITimeAffected timeObject)
    {
        _timeObjectsInRadius.Add(timeObject);
    }

    void RemoveObject(ITimeAffected timeObject)
    {
        _timeObjectsInRadius.Remove(timeObject);
    }
}
