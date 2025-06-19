using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Graph : MonoBehaviour
{
    [SerializeField] private GraphNode[] _nodes = new GraphNode[0];
    public GraphNode[] Nodes => _nodes;

    public void AddNewNode(NodeType selection, Vector2 position)
    {
        List<GraphNode> tempNodes = new List<GraphNode>(_nodes);

        int id = GetUniqueNodeId();
        GraphNode node = new GraphNode(selection, id, position);

        tempNodes.Add(node);
        _nodes = tempNodes.ToArray();
    }

    private int GetUniqueNodeId()
    {
        int counter = 0;
        int id = UnityEngine.Random.Range(1000, 9999);
        while(TryGetNodeFromId(id, out GraphNode node))
        {
            id = UnityEngine.Random.Range(1000, 9999);
            
            counter++;
            if (counter > 1000)
            {
                throw new Exception("Could not find a unique id");
            }
        }
        return id;
    }

    public bool TryGetNodeFromId(int id, out GraphNode node)
    {
        node = null;
        for (int i = 0; i < Nodes.Length; i++)
        {
            if (Nodes[i].Id == id)
            {
                node = Nodes[i];
                return true;
            }
        }
        return false;
    }
}
