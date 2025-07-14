using UnityEngine;

public class SaveData
{
    [SerializeField] private float _musicVolume = 0.5f;
    [SerializeField] private float _sfxVolume = 0.5f;
    [SerializeField] private float _dialogueVolume = 0.5f;

    public float MusicVolume { get { return _musicVolume; } set { _musicVolume = Mathf.Clamp(value, 0.0f, 1.0f); } }
    public float SfxVolume { get { return _sfxVolume; } set { _sfxVolume = Mathf.Clamp(value, 0.0f, 1.0f); } }
    public float DialogueVolume { get { return _dialogueVolume; } set { _dialogueVolume = Mathf.Clamp(value, 0.0f, 1.0f); } }
}
