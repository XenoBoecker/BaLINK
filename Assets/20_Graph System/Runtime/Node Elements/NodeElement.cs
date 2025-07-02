using System;
using UnityEngine;

public abstract class NodeElement : ScriptableObject
{
    [SerializeField] private GameObject _referencedObject;
    [SerializeField, HideInInspector] private GameObject _player;
    [SerializeField, HideInInspector] private bool _isUnfolded = true;
    [SerializeField, HideInInspector] private int _connectedOutputIndex;

    public GameObject ReferencedObject => _referencedObject;
    public GameObject Player => GetPlayerObject();
    public bool IsUnfolded => _isUnfolded;
    public int ConnectedOutputIndex => _connectedOutputIndex;

    public abstract bool ConditionIsMet();
    public abstract void TriggerEffect();
    public abstract string GetName();
    public abstract void OnExitNode();

    public override string ToString()
    {
        return GetName();
    }
    public virtual void Initialize(GameObject referencedObject, int connectedOutputIndex)
    {
        _referencedObject = referencedObject;
        _connectedOutputIndex = connectedOutputIndex;
    }

    public void SetUnfolded(bool value)
    {
        _isUnfolded = value;
    }

    private GameObject GetPlayerObject()
    {
        if (_player != null) { return _player; }

        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player == null)
        {
            throw new Exception("Could not find player in the scene. Has the player been tagged as 'Player'?");
        }
        return _player;
    }

    public void SetConnectedOutputIndex(int index)
    {
        _connectedOutputIndex = index;
    }
}
