using UnityEngine;
using GameEvents;

public class BlinkUIVisualizer : MonoBehaviour
{
    [SerializeField] GameObject blinkUI;
    [SerializeField] float blinkDuration = 0.5f;

    [SerializeField] private AudioClip blinkSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool playSoundOnBlink = true;

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
        if (blinkUI != null) blinkUI.SetActive(true);
        else Debug.Log("No blinkUI", this);

        blinkTimer = blinkDuration;

        if (playSoundOnBlink && audioSource != null && blinkSound != null)
        {
            audioSource.PlayOneShot(blinkSound);
        }
    }
}
