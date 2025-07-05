using UnityEngine;

public class AudioSystemClip : ScriptableObject
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private float _volume;
}
