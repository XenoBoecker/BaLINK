using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Graph))]
public class GraphEditor : Editor
{
    GraphEditorWindow _window;

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Edit"))
        {
            _window = CreateInstance<GraphEditorWindow>();
            _window.CreateWindow((target as Graph));
        }

        base.OnInspectorGUI();
    }
}
