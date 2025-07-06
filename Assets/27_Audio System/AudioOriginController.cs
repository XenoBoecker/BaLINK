using GameEvents;
using System.Collections;
using UnityEngine;

public class AudioOriginController : MonoBehaviour
{
    [SerializeField] private AudioSystemClip _clip;
    [SerializeField] private float _cooldown = 0;

    bool _isOnCooldown;

    public void PlayClip()
    {
        if (_isOnCooldown) { return; }
        ObjectEvents.PlayAudio(_clip, transform.position);
        StartCoroutine(DoCooldown());
    }

    private IEnumerator DoCooldown()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(_cooldown);
        _isOnCooldown = false;
    }
}
