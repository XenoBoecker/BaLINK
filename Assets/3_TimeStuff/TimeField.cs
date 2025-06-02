using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TimeField : MonoBehaviour
{
    [SerializeField] float timeScale = 0.5f;

    List<TimeAffected> timeObjectsInRadius = new List<TimeAffected>();

    bool timeSlowActive;

    public void Toggle()
    {
        if (timeSlowActive)
        {
            BackToNormalSpeed();
        }
        else
        {
            SlowDownTime();
        }

        Debug.Log("Toggle Time");
    }

    void SlowDownTime()
    {
        foreach (TimeAffected timeObject in timeObjectsInRadius)
        {
            timeObject.SetTimeScale(timeScale);
        }
        timeSlowActive = true;
    }

    void BackToNormalSpeed()
    {
        foreach (TimeAffected timeObject in timeObjectsInRadius)
        {
            timeObject.SetTimeScale(1);
        }
        timeSlowActive = false;
    }

    void AddObject(TimeAffected timeObject)
    {
        timeObjectsInRadius.Add(timeObject);
    }

    void RemoveObject(TimeAffected timeObject)
    {
        timeObjectsInRadius.Remove(timeObject);
    }


    private void OnTriggerEnter(Collider collision)
    {
        TimeAffected timeObject = collision.GetComponent<TimeAffected>();

        if (timeObject == null) return;

        if (timeSlowActive)
        {
            timeObject.SetTimeScale(timeScale);
        }

        AddObject(timeObject);
    }

    private void OnTriggerExit(Collider collision)
    {
        TimeAffected timeObject = collision.GetComponent<TimeAffected>();

        if (timeObject == null) return;

        if (timeSlowActive)
        {
            timeObject.SetTimeScale(1);
        }

        RemoveObject(timeObject);
    }
}
