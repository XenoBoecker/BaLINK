using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// Toggles whether the selected Game Objects are active or not based on the distance between the Main Camera and
// the object with this script applied to it.
//

public class ToggleByCameraDistance : MonoBehaviour
{
    [SerializeField] GameObject[] _gameObjects;
    [SerializeField] float _maxCameraDistance = 25.0f;
    [SerializeField] float _updateTime = 0.5f; // Update every 0.5s


    private GameObject _mainCamera;

    private void Awake()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Start()
    {
        InvokeRepeating("timedUpdate", 0.0f, _updateTime);
    }

    private void timedUpdate()
    {
        bool isInRange = Vector3.Distance(_mainCamera.transform.position, transform.position) < _maxCameraDistance;

        foreach (GameObject gameObject in _gameObjects)
        {
            gameObject.SetActive(isInRange);
        }
    }
}
