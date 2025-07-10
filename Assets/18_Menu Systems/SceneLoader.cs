using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByString(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
