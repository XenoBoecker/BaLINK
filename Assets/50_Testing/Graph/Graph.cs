using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Graph : MonoBehaviour
{
    [SerializeField] private GraphNode[] _nodes = new GraphNode[0];
    public GraphNode[] Nodes => _nodes;

    public void AddNode(GraphNode node)
    {
        List<GraphNode> tempNodes = new List<GraphNode>(_nodes);
        tempNodes.Add(node);
        _nodes = tempNodes.ToArray();
    }

    public GameObject obj;

    private void OnEnable()
    {
        GraphNode node = new GraphNode(GraphNode.NodeType.EFFECT);
        node.NodeComponents = new Component[] { (Component)obj.GetComponent<IGraphEffectable>() };

        EditorUtility.SetDirty(gameObject);
    }
}
