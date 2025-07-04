using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainCarAdditiveLoader : MonoBehaviour
{
    [SerializeField] public string[] _scenes;

    private void Awake()
    {
        for (int i = 0; i < _scenes.Length; i++)
        {
            SceneManager.LoadScene(_scenes[i], LoadSceneMode.Additive);
        }
    }
}
