using System;
using System.Collections;
using UnityEngine;

public class MusicSystemManager : MonoBehaviour
{
    [SerializeField] private AudioClip _initialClip;
    [SerializeField] private float _initialFadeInSpeed;
    [SerializeField] private GameObject _sourcePrefab;
    public static MusicSystemManager Instance;

    private AudioSource _currentSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Too many instance");
        } 
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        FadeInTrack(_initialClip, _initialFadeInSpeed);
    }

    private void Update()
    {
        if (_currentSource != null)
        {
            _currentSource.volume = SaveSystem.Data.MusicVolume;
        }
    }

    public void FadeInTrack(AudioClip clip, float fadeDuration)
    {
        AudioSource source = Instantiate(_sourcePrefab, transform).GetComponent<AudioSource>();
        source.transform.localPosition = Vector3.zero;
        source.clip = clip;
        source.volume = 0;
        source.Play();

        StartCoroutine(FadeInSource(source, fadeDuration));
        if (_currentSource != null)
        {
            StartCoroutine(FadeOutSource(_currentSource, fadeDuration));
            _currentSource = null;
        }
    }

    public void FadeOutCurrentTrack(float fadeDuration)
    {
        if (_currentSource != null)
        {
            StartCoroutine(FadeOutSource(_currentSource, fadeDuration));
        }
    }

    private IEnumerator FadeOutSource(AudioSource source, float fadeDuration)
    {
        float initalVolume = source.volume;

        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            source.volume = initalVolume * (1 - i / fadeDuration);
            yield return null;
        }

        Destroy(source.gameObject);
    }

    private IEnumerator FadeInSource(AudioSource source, float fadeDuration)
    {
        float targetVolume = SaveSystem.Data.MusicVolume;

        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            source.volume = targetVolume * (i / fadeDuration);
            yield return null;
        }

        _currentSource = source;
    }
}
