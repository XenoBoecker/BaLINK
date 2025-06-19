using System;
using UnityEngine;

public abstract class NodeElement : ScriptableObject
{
    [SerializeField] private GameObject _referencedObject;
    [SerializeField, HideInInspector] private bool _isUnfolded;

    public GameObject ReferencedObject => _referencedObject;
    public bool IsUnfolded => _isUnfolded;

    public virtual void Initialize(GameObject referencedObject)
    {
        _referencedObject = referencedObject;
    }
    public void SetUnfolded(bool value)
    {
        _isUnfolded = value;
    }

    public abstract bool ConditionIsMet();
    public abstract void TriggerEffect();
    public abstract string GetName();
}
