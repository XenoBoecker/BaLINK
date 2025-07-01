using System;
using UnityEngine;

[NodeElement(NodeType.Condition, NodeType.IfElse)]
[RequireComponent(typeof(MeshRenderer))]
public class PlayerLookingAtObject : Condition
{
    [SerializeField] private LayerMask _enviromentMask;
    private MeshRenderer _renderer;

    public override bool ConditionIsMet()
    {
        Physics.queriesHitBackfaces = false;
        Physics.queriesHitTriggers = false;

        if (_renderer == null)
        {
            _renderer = ReferencedObject.GetComponent<MeshRenderer>();
        }

        if (!_renderer.isVisible != _invertCondition)
        {
            return _invertCondition != false;
        }

        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3[] targets = new Vector3[] { ReferencedObject.transform.position };
        for (int i = 0; i < targets.Length; i++)
        {
            if (CastHitEnviroments(rayOrigin, targets[i]))
            {
                return _invertCondition != false;
            }
        }
        
        return _invertCondition != true;
    }

    private bool CastHitEnviroments(Vector3 rayOrigin, Vector3 rayTarget)
    {
        Vector3 direction = rayOrigin - rayTarget;
        return Physics.Raycast(new Ray(rayTarget, direction.normalized), direction.magnitude, _enviromentMask);
    }

    public override string GetName()
    {
        return "Player is Looking at Object";
    }
}
