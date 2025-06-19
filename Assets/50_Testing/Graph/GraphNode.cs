using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GraphNode
{
    [SerializeReference] private int _id;
    [SerializeField] private NodeType _type;
    [SerializeField] private Vector2 _position;
    [SerializeReference] private int _nextNodeId;
    [SerializeReference] private NodeElement[] _elements;

    public int Id => _id;
    public NodeType Type => _type;
    public Vector2 Position => _position;
    public int NextNodeId => _nextNodeId;
    public NodeElement[] Elements => _elements;

    public GraphNode(NodeType type, int id)
    {
        _type = type;
        _id = id;
        _elements = new NodeElement[0];
        _nextNodeId = -1;
    }

    public GraphNode(NodeType type, int id, Vector2 position) : this(type, id)
    {
        _position = position;
    }

    public void ChangePosition(Vector2 change)
    {
        _position += change;
    }

    public void AddElement(NodeElement element)
    {
        List<NodeElement> tempElement = new List<NodeElement>(Elements);
        tempElement.Add(element);
        _elements = tempElement.ToArray();
    }

    public void SetNextNodeId(int nodeId)
    {
        _nextNodeId = nodeId;
    }
}
