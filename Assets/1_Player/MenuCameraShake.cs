using UnityEngine;

public class MenuCameraShake : MonoBehaviour
{
    [SerializeField] private float _frequencey;
    [SerializeField] private float _strength;

    float _time = 0;
    Vector3 _initialPosition;

    private void Awake()
    {
        _initialPosition = transform.position;
    }

    void Update()
    {
        _time += Time.deltaTime * _frequencey;
        transform.position = _initialPosition + new Vector3(0, Mathf.PerlinNoise1D(_time) * _strength, 0);
    }
}
