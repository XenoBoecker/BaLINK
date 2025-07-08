using UnityEngine;

[NodeElement(NodeType.Effect)]
[RequireComponent(typeof(PauseMenu))]
public class ChangeScene : Effect
{
    [SerializeField]
    private string _sceneName;

    public override string GetName()
    {
        return "Change Scene";
    }

    public override void TriggerEffect()
    {
        if (ReferencedObject.TryGetComponent(out PauseMenu pauseMenu))
        {
            pauseMenu.ChangeScene(_sceneName);
        }
    }
}
