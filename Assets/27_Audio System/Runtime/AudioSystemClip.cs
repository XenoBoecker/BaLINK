using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Clip", menuName = "Audio Clip", order = -1000)]
public class AudioSystemClip : ScriptableObject
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private GameObject _audioSourcePrefab;

    public AudioClip Clip => _clip;
    public GameObject AudioSourcePrefab => _audioSourcePrefab;
}
