using UnityEngine;

public class FollowPlayerAlongCar : MonoBehaviour
{
    [SerializeField] private float _offset;
    private GameObject _player;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, _player.transform.position.z + _offset);
    }
}
