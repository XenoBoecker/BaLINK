using GameEvents;
using System.Collections;
using UnityEngine;

public class AudioSystemManager : MonoBehaviour
{
    private void OnEnable()
    {
        ObjectEvents.OnPlayAudio += PlayAudio;
    }

    private void OnDisable()
    {
        ObjectEvents.OnPlayAudio -= PlayAudio;
    }

    private void PlayAudio(AudioSystemClip clip, Vector3 position)
    {
        StartCoroutine(CreateSourceAndPlay(clip, position));
    }

    private IEnumerator CreateSourceAndPlay(AudioSystemClip clip, Vector3 position)
    {
        if (clip.Clip == null || clip.AudioSourcePrefab == null) { yield break; }

        GameObject obj = Instantiate(clip.AudioSourcePrefab, transform);
        obj.transform.position = position;
        obj.transform.rotation = Quaternion.identity;

        AudioSource source = obj.GetComponent<AudioSource>();
        source.clip = clip.Clip;
        source.Play();

        float volume = clip.VolumeMultiplier;
        switch (clip.ClipType)
        {
            case AudioSystemClipType.SFX:
                volume *= SaveSystem.Data.SfxVolume;
                break;
            case AudioSystemClipType.Music:
                volume *= SaveSystem.Data.MusicVolume;
                break;
            case AudioSystemClipType.Dialogue:
                volume *= SaveSystem.Data.DialogueVolume;
                break;
        }

        source.volume = volume;
        source.pitch = source.pitch + Random.Range(-clip.PitchVariation, clip.PitchVariation);

        yield return new WaitUntil(() => { return !source.isPlaying; });

        Destroy(obj);
    }
}