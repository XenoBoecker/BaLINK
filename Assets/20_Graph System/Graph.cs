using GameEvents;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Custom/Graph")]
public class Graph : MonoBehaviour
{
    [SerializeField, HideInInspector] private int _entryNodeId;
    [SerializeField, HideInInspector] private GraphNode[] _nodes = new GraphNode[0];
    [SerializeField, HideInInspector] private Vector2 _center;

    public Vector2 Center => _center;
    public int EntryNodeId => _entryNodeId;
    public GraphNode[] Nodes => _nodes;

    private GraphNode _currentNode;
    private bool _usingWaitForBlink = true;

    private void OnEnable()
    {
        InputEvents.onPlayerBlinked += OnPlayerBlinked;
    }
    private void OnDisable()
    {
        InputEvents.onPlayerBlinked -= OnPlayerBlinked;   
    }

    private void Start()
    {
        EnterGraph();
    }

    private void Update()
    {
        if (!_usingWaitForBlink)
        {
            //do node always returns true when node is of type effect so it will trigger the effect once and then move to the next node.
            //if it's a condition node then DoNode will return true when conditons are met 
            if (_currentNode.ConditionElementConditionsMet())
            {
                MoveToNextNode();
            }
        }
    }

    private void EnterGraph()
    {
        //get the endtry node
        if (!TryGetNodeFromId(_entryNodeId, out GraphNode entryNode)) { return; }
        _currentNode = entryNode;
        _usingWaitForBlink = _currentNode.WaitForBlink;
    }

    private void MoveToNextNode()
    {
        if (TryGetNodeFromId(_currentNode.NextNodeId, out GraphNode foundNode))
        {
            _currentNode = foundNode;
            _usingWaitForBlink = _currentNode.WaitForBlink;
            foundNode.TriggerAllEffectElements();

            if (foundNode.Type == NodeType.Effect)
            {
                if (TryGetNodeFromId(_currentNode.NextNodeId, out GraphNode nextNode))
                {
                    if (nextNode.Type != NodeType.Effect)
                    {
                        MoveToNextNode();
                        return;
                    }
                }
            }
        }
    }

    private void OnPlayerBlinked()
    {
        if (!_usingWaitForBlink) { return; }

        if (_currentNode.ConditionElementConditionsMet())
        {
            MoveToNextNode();
        }
    }

    public void AddNewNode(NodeType nodeType, Vector2 position)
    {
        int id = GetUniqueNodeId();
        GraphNode node = new GraphNode(nodeType, id, position);

        List<GraphNode> tempNodes = new List<GraphNode>(_nodes);
        tempNodes.Add(node);
        _nodes = tempNodes.ToArray();

        if (nodeType == NodeType.Entry)
        {
            if (_entryNodeId != -1)
            {
                RemoveNodeById(_entryNodeId);
            }

            _entryNodeId = node.Id;
        } 
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

    public void RemoveNodeById(int id)
    {
        List<GraphNode> tempNodes = new List<GraphNode>(Nodes);
        for (int i = 0; i < Nodes.Length; i++)
        {
            if (Nodes[i].Id == id)
            {
                tempNodes.Remove(Nodes[i]);
                break;
            }
        }

        _nodes = tempNodes.ToArray();
    }

    public void ChangeGraphCenter(Vector2 change)
    {
        _center += change;
    }
}
