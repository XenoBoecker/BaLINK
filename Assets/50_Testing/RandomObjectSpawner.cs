using System;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    [SerializeField] RandomMovingObject _objectPrefab;

    [SerializeField] int _objectsPerSecond;

    float _timer;

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer < 0)
        {
            _timer = 1;
            SpawnObjects(_objectsPerSecond);
        }
    }

    private void SpawnObjects(int objectsPerSecond)
    {
        for (int i = 0; i < objectsPerSecond; i++)
        {
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        if (_objectPrefab != null)
        {
            Instantiate(_objectPrefab);
        }
    }
}
