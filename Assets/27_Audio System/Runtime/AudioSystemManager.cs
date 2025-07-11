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
        source.volume = clip.VolumeMultiplier;
        source.pitch = source.pitch + Random.Range(-clip.PitchVariation, clip.PitchVariation);

        yield return new WaitUntil(() => { return !source.isPlaying; });

        Destroy(obj);
    }
}