using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Clip", menuName = "Audio Clip", order = -1000)]
public class AudioSystemClip : ScriptableObject
{
    [SerializeField] private AudioSystemClipType _clipType;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private GameObject _audioSourcePrefab;
    [SerializeField, Range(0f, 1f)] private float _volumeMultiplier = 1;
    [SerializeField] private float _pitchVariation = 0.0f;

    public AudioClip Clip => _clip;
    public GameObject AudioSourcePrefab => _audioSourcePrefab;
    public float VolumeMultiplier => _volumeMultiplier;
    public float PitchVariation => _pitchVariation;
    public AudioSystemClipType ClipType => _clipType;

    internal void Initialize(AudioSystemClipType clipType, AudioClip clip, GameObject audioSourcePrefab)
    {
        _clipType = clipType;
        _clip = clip;
        _audioSourcePrefab = audioSourcePrefab;
    }
}
