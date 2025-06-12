using UnityEngine;
using GameEvents;

public class BlinkUIVisualizer : MonoBehaviour
{
    [SerializeField] GameObject blinkUI;
    [SerializeField] float blinkDuration = 0.5f;

    float blinkTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputEvents.onPlayerBlinked += OnPlayerBlinked;

        blinkUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (blinkTimer > 0f)
        {
            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0f)
            {
                blinkUI.SetActive(false);
            }
        }
    }

    void OnPlayerBlinked()
    {
        blinkUI.SetActive(true);
        blinkTimer = blinkDuration;
    }
}
