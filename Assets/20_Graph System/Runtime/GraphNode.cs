using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class GraphNode
{
    [SerializeReference] private int _id;
    [SerializeField] private NodeType _type;
    [SerializeField] private Vector2 _position;
    [SerializeReference] private int[] _nextNodeIds;
    [SerializeField] private bool _waitForBlink = true;
    [SerializeReference] private NodeElement[] _elements;
    [SerializeReference] private bool _isUnfolded = true;
    [SerializeField] private int _outputCount;

    public int Id => _id;
    public NodeType Type => _type;
    public Vector2 Position => _position;
    public int[] NextNodeIds => _nextNodeIds;
    public bool WaitForBlink => _waitForBlink;
    public NodeElement[] Elements => _elements;
    public bool IsUnfolded => _isUnfolded;
    public int OutputCount => _outputCount;

    public GraphNode(NodeType type, int id, int outputCount = 1)
    {
        _type = type;
        _id = id;
        _elements = new NodeElement[0];
        
        _outputCount = outputCount;

        _nextNodeIds = new int[OutputCount];
        for (int i = 0; i < NextNodeIds.Length; i++)
        {
            _nextNodeIds[i] = -1;
        }
    }

    public GraphNode(NodeType type, int id, Vector2 position, int outputCount = 1) : this(type, id, outputCount)
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

    public void RemoveElement(NodeElement element)
    {
        List<NodeElement> tempElement = new List<NodeElement>(Elements);

        tempElement.Remove(element);
        
        _elements = tempElement.ToArray();
    }

    public void SetNextNodeId(int nodeId, int connectedOutputIndex)
    {
        _nextNodeIds[connectedOutputIndex] = nodeId;
    }

    public void SetUnfolded(bool value)
    {
        _isUnfolded = value;
    }

    public void SetWaitForBlink(bool value)
    {
        _waitForBlink = value;
    }

    public void TriggerAllEffectElements()
    {
        if (Type != NodeType.Effect) { return; }
        foreach (NodeElement element in Elements)
        {
            element.TriggerEffect();
        }
    }

    public bool ConditionElementConditionsMet(out int outputIndexTrue)
    {
        outputIndexTrue = 0;
        if (Type != NodeType.Condition && Type != NodeType.IfElse) { return true; }

        for (int i = 0; i < OutputCount; i++)
        {
            if (ConditionsMetForOutput(i))
            {
                outputIndexTrue = i;
                return true;
            }
        }

        return false;

        bool ConditionsMetForOutput(int index)
        {
            int numberOfElementsForOutput = 0;
            foreach (NodeElement element in Elements)
            {
                if (element.ConnectedOutputIndex != index) { continue; }
                numberOfElementsForOutput++;
                if (!element.ConditionIsMet())
                {
                    return false;
                }
            }

            if (numberOfElementsForOutput == 0)
            {
                Debug.LogWarning($"No condition set for output at index {index}. That output was picked since no conditons apply");
            }
            return true;
        }
    }

    internal void ExitNode()
    {
        foreach (NodeElement element in Elements)
        {
            element.OnExitNode();
        }
    }

    public void RemoveNoneElements()
    {
        List<NodeElement> tempElements = new List<NodeElement>(_elements);

        for (int i = _elements.Length - 1; i >= 0; i --)
        {
            if (tempElements[i] == null)
            {
                tempElements.RemoveAt(i);
            }
        }

        _elements = tempElements.ToArray();
    }
}
