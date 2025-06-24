using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float secondsUntilExplosion = 300;

    [SerializeField] private ReplaceObjectInteractable[] wiresToCut;

    private float timeLeft;
    private float timeScale = 1;

    bool isDefused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeLeft = secondsUntilExplosion;

        for (int i = 0; i < wiresToCut.Length; i++)
        {
            wiresToCut[i].OnInteracted += CheckBombDefused;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isDefused)
        {
            return;
        }

        timeLeft -= Time.deltaTime * timeScale;

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

    public float GetCurrentTimeLeft()
    {
        return timeLeft;
    }

    private void CheckBombDefused()
    {
        for (int i = 0; i < wiresToCut.Length; i++)
        {
            if (!wiresToCut[i].HasBeenInteractedWithThisGame) return;
        }

        isDefused = true;
        Debug.Log("Bomb has been defused!");
    }

    private void Explode()
    {
        Debug.Log("Bomb exploded: " + gameObject.name);
    }
}
