using UnityEngine;

public class AudioSystemManager :MonoBehaviour
{
    private GameObject _audioSourcePrefab;

    private void Awake()
    {
        Resources.Load("Audio System Source");
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
}
