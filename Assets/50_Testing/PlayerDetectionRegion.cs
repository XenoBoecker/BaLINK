using UnityEngine;
using BetterAttributes;

public class PlayerDetectionRegion : MonoBehaviour
{
    [SerializeField, ReadOnly] private float _playerContainedDuration;
    [SerializeField, ReadOnly] private bool _playerInRegion;
    public float PlayerContainedDuration => _playerContainedDuration;
    public bool PlayerInRegion => _playerInRegion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _playerInRegion = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            _playerContainedDuration += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _playerInRegion = false;
        }
    }
}
