using UnityEngine;

[NodeElement(NodeType.Effect)]
public class FadeMusic : Effect
{
    [SerializeField] private FadeMode _fadeMode;
    [SerializeField] private float _fadeSpeed;
    [SerializeField] private AudioClip _track;

    public override string GetName()
    {
        return "Fade In Music";
    }

    public override void TriggerEffect()
    {
        if (MusicSystemManager.Instance == null) { return; }

        switch (_fadeMode)
        {
            case FadeMode.FadeIn:
                MusicSystemManager.Instance.FadeInTrack(_track, _fadeSpeed);
                break;
            
            case FadeMode.FadeOut:
                MusicSystemManager.Instance.FadeOutCurrentTrack(_fadeSpeed);
                break;
        }
    }

    private enum FadeMode
    {
        FadeIn,
        FadeOut
    }
}
