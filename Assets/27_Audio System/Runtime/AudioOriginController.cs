using GameEvents;
using System.Collections;
using UnityEngine;

public class AudioOriginController : MonoBehaviour
{
    [SerializeField] private float _cooldown = 0;

    bool _isOnCooldown;

    public void PlayClip(AudioSystemClip clip)
    {
        if (_isOnCooldown) { return; }
        ObjectEvents.PlayAudio(clip, transform.position);
        StartCoroutine(DoCooldown());
    }

    private IEnumerator DoCooldown()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(_cooldown);
        _isOnCooldown = false;
    }
}
