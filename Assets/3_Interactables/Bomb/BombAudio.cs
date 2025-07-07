using UnityEngine;

public class BombAudio : MonoBehaviour
{
    [SerializeField] private AudioClip bombTickingSound;

    private Bomb _bomb;
    private AudioSource audioSource;

    int lastIntegerTime;

    private void Awake()
    {
        _bomb = GetComponent<Bomb>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        lastIntegerTime = (int)_bomb.GetCurrentTimeLeft();
    }

    private void Update()
    {
        if((int)_bomb.GetCurrentTimeLeft() != lastIntegerTime)
        {
            lastIntegerTime = (int)_bomb.GetCurrentTimeLeft();
            PlayBombTickingSound();
        }
    }

    private void PlayBombTickingSound()
    {
        if (audioSource.isPlaying) return;
        audioSource.clip = bombTickingSound;
        audioSource.Play();
    }
}
