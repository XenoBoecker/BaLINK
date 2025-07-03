using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneSequenceEvent : IntroSequenceEvent
{
    [SerializeField] private string _sceneToLoad;

    public override void TriggerSequenceEvent()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
    
    public override bool IsFinished()
    {
        return true;
    }
}
