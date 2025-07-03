using UnityEngine;

public class BombTickingLight : MonoBehaviour
{
    [SerializeField] private Light lightSource;
    [SerializeField] private Bomb _bomb;

    [SerializeField] private float minLightIntensity = 1f;
    [SerializeField] private float maxLightIntensity = 2f;

    float baseIntensity;
    float intensityAmplitude;

    private void Start()
    {
        baseIntensity = (minLightIntensity + maxLightIntensity) / 2f;
        intensityAmplitude = maxLightIntensity - baseIntensity;
    }

    private void Update()
    {
        lightSource.intensity = baseIntensity + intensityAmplitude * Mathf.Sin(Time.time * _bomb.GetTimeScale() * 2 * Mathf.PI);
    }
}
