using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GraphNode
{
    [SerializeField] private Vector2 _position;
    [SerializeField] private NodeType _type;
    [SerializeReference] private GraphNode _nextNode;

    public NodeType Type => _type;
    public Vector2 Position => _position;
    public Component[] NodeComponents;
    [SerializeReference] public List<Condition> conditions = new List<Condition>();

    public GraphNode(NodeType type) 
    {
        _type = type;
        NodeComponents = new Component[0];
    }

    public void ChangePosition(Vector2 change)
    {
        _position += change;
    }

    public void AddComponent(Component component)
    {
        List<Component> tempComponents = new List<Component>(NodeComponents);
        tempComponents.Add(component);
        NodeComponents = tempComponents.ToArray();
    }

    public enum NodeType
    {
        EFFECT,
        CONDITION,
    }
}
