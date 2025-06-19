using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Graph))]
public class GraphEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Edit"))
        {
            GraphEditorWindow.CreateWindow((target as Graph));
        }

        base.OnInspectorGUI();
    }
}
