using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderController : MonoBehaviour
{
    public void LoadSceneByString(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
