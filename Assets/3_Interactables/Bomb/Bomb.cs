using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WrongButtonPressedEffect
{
    public bool halveTime;
    public bool setTimerTimescale;
    public float newTimerTimescale;
    public bool subtractTime;
    public float timeToSubtract;
}

public class Bomb : MonoBehaviour
{
    [SerializeField] private float secondsUntilExplosion = 300;

    [SerializeField] private Interactable[] correctButtons, wrongButtons;

    [SerializeField] private List<WrongButtonPressedEffect> wrongButtonPressedEffects;

    private float timeLeft;
    private float timeScale = 1;

    bool isDefused = false;

    int wrongButtonPressedCount = 0;

    public event Action OnBombDefused;
    public event Action OnBombExploded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeLeft = secondsUntilExplosion;

        for (int i = 0; i < correctButtons.Length; i++)
        {
            correctButtons[i].OnInteracted += CheckBombDefused;
        }

        for (int i = 0;i < wrongButtons.Length; i++)
        {
            wrongButtons[i].OnInteracted += WrongButtonPressed;
        }
    }

    private void WrongButtonPressed()
    {
        int currentWrongButtonPressedCount = 0;

        for (int i = 0; i < wrongButtons.Length; i++)
        {
            if (wrongButtons[i].HasBeenInteractedWithThisGame) currentWrongButtonPressedCount++;
        }

        if(currentWrongButtonPressedCount == wrongButtonPressedCount)
        {
            return;
        }

        wrongButtonPressedCount = currentWrongButtonPressedCount;

        ActivateWrongButtonEffect(wrongButtonPressedCount);
    }

    private void ActivateWrongButtonEffect(int currentWrongButtonPressedCount)
    {
        WrongButtonPressedEffect effect = wrongButtonPressedEffects[currentWrongButtonPressedCount-1];
        if (effect.halveTime)
        {
            timeLeft /= 2;
        }
        if (effect.setTimerTimescale)
        {
            SetTimeScale(effect.newTimerTimescale);
        }
        if (effect.subtractTime)
        {
            timeLeft -= effect.timeToSubtract;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timeScale != 1) print("Time scale is not 1, current time scale: " + timeScale);
        timeLeft -= Time.deltaTime * timeScale;

        if (isDefused)
        {
            return;
        }

        if (timeLeft <= 0)
        {
            Explode();
        }
    }

    public void SetTimeScale(float newTimeScale)
    {
        timeScale = newTimeScale;
    }

    public void SetCurrentTimeLeft(float newTimeLeft)
    {
        timeLeft = newTimeLeft;
    }

    public void AddTime(float additionalTime)
    {
        timeLeft += additionalTime;
    }

    public float GetCurrentTimeLeft()
    {
        return timeLeft;
    }

    private void CheckBombDefused()
    {
        for (int i = 0; i < correctButtons.Length; i++)
        {
            if (!correctButtons[i].HasBeenInteractedWithThisGame) return;
        }

        isDefused = true;
        Debug.Log("Bomb has been defused!");

        OnBombDefused?.Invoke();
    }

    private void Explode()
    {
        Debug.Log("Bomb exploded: " + gameObject.name);

        OnBombExploded?.Invoke();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < correctButtons.Length; i++)
        {
            correctButtons[i].OnInteracted -= CheckBombDefused;
        }

        for (int i = 0; i < wrongButtons.Length; i++)
        {
            wrongButtons[i].OnInteracted -= WrongButtonPressed;
        }
    }

    internal float GetTimeScale()
    {
        return timeScale;
    }
}
