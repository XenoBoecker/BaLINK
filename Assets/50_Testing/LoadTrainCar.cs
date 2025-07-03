using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTrainCar : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad;
    GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        _player.transform.position = Vector3.zero;
        SceneManager.LoadScene(_sceneToLoad);
    }
}
