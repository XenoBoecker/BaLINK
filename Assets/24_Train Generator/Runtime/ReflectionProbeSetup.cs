using UnityEngine;

[RequireComponent(typeof(ReflectionProbe))]
public class ReflectionProbeSetup : MonoBehaviour
{
    ReflectionProbe _probe;

    private void Awake()
    {
        _probe = GetComponent<ReflectionProbe>();
    }

    public void SetProbeBounds(TrainCarGenerator generator, int index)
    {
        if (generator == null || generator.NumberOfReflectionProbes  <= 0) { return; }

        if (_probe == null)
        {
            _probe = GetComponent<ReflectionProbe>();
        }

        float length = (generator.TrainCarLength + 2) / generator.NumberOfReflectionProbes;

        _probe.size = new Vector3(generator.ProbeWidth, generator.ProbeHeight, length);
        transform.localPosition = new Vector3(0, generator.ProbeHeight * 0.5f, length * index + length * 0.5f - 1);
    }
}
