using UnityEngine;

public class PlayerDetectionRegion : MonoBehaviour
{
    [SerializeField] private float _playerContainedDuration;
    [SerializeField] private bool _playerInRegion;
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
