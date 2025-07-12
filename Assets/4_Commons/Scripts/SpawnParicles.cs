using UnityEngine;

public class SpawnParicles : MonoBehaviour
{
    public void SpawnParticleSystem(ParticleSystem system)
    {
        GameObject sparks = Instantiate(system, transform).gameObject;
        sparks.transform.localPosition = new Vector3(0, 0.075f, 0);
        sparks.transform.localEulerAngles = new Vector3(-90, 0, 0);
    }
}
