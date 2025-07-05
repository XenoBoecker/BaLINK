using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ParallaxEffect))]
public class ParallaxEffectEditor : Editor
{
    private bool _isPreviewing;
    private bool _wasPreviewing;

    public override void OnInspectorGUI()
    {
        ParallaxEffect effect = target as ParallaxEffect;
        GUIStyle toggleStyle = GUI.skin.button;
        _wasPreviewing = _isPreviewing;
        _isPreviewing = GUILayout.Toggle(_isPreviewing, new GUIContent("Preview"), toggleStyle);
        if (_isPreviewing)
        {
            if (!_wasPreviewing)
            {
                effect.Initialize();
            }
            effect.Preview();
            HandleUtility.Repaint();
        } 
        else if (_wasPreviewing)
        {
            effect.Cleanup();
        }

        base.OnInspectorGUI();
    }
}
