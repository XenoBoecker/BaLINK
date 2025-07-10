using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Clip", menuName = "Audio Clip", order = -1000)]
public class AudioSystemClip : ScriptableObject
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private GameObject _audioSourcePrefab;
    [SerializeField, Range(0f, 1f)] private float _volumeMultiplier = 1;

    public AudioClip Clip => _clip;
    public GameObject AudioSourcePrefab => _audioSourcePrefab;
    public float VolumeMultiplier => _volumeMultiplier;
}
