using System.Collections;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public void Initialize(float lifetime)
    {
        StartCoroutine(DelfDestructAfterDelay(lifetime));
    }

    private IEnumerator DelfDestructAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
