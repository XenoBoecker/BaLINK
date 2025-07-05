using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Graph))]
public class GraphEditor : Editor
{
/*    static Graph _savedGraph;
    static string _json;*/

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Edit"))
        {
            GraphEditorWindow.CreateWindow((target as Graph));
        }

        base.OnInspectorGUI();
/*
        Graph graph = target as Graph;
        if (GUILayout.Button("Copy"))
        {
            _json = JsonUtility.ToJson(graph);
            //_savedGraph = graph;
        }

        if (GUILayout.Button("Paste"))
        {
            Debug.Log(_json);
            Graph copiedGraph = JsonUtility.FromJson<Graph>(_json);   
            graph.SetData(copiedGraph);
        }*/
    }
}
