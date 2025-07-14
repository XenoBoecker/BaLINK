using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Graph))]
public class GraphEditor : Editor
{
/*    static Graph _savedGraph;
    static string _json;*/

    public override void OnInspectorGUI()
    {
        Graph graph = (target as Graph);

        if (GUILayout.Button("Edit"))
        {
            GraphEditorWindow.CreateWindow(graph);
        }


        if (GUILayout.Button("Repair"))
        {
            for (int i = 0; i < graph.Nodes.Length; i++)
            {
                graph.Nodes[i].RemoveNoneElements();
            }
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
