using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    [SerializeField] RandomMovingObject objectPrefab;

    [SerializeField] float objectsPerSecond;

    float timer;

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        if (objectPrefab != null)
        {
            Instantiate(objectPrefab);
        }
    }
}
