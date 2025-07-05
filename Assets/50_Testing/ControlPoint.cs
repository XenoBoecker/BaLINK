using System;
using UnityEngine;

[System.Serializable]
public class ControlPoint
{
    [SerializeField] private Vector3 _position;
    [SerializeField] private Vector3 _surfaceNormal;
    [SerializeField] private Vector3 _tangentBack;
    [SerializeField] private Vector3 _tangentFront;
    [SerializeField] private float _radius;

    public Vector3 Position => _position;
    public float Radius => _radius;
    public Vector3 SurfaceNormal => _surfaceNormal;
    public Vector3 TangentBack => _tangentBack;
    public Vector3 TangentFront => _tangentFront;

    public Vector3 GetOffsetPosition(float radius)
    {
        return _position + SurfaceNormal * (radius + Radius);
    }

    public void ChangePosition(float radius, Vector3 change)
    {
        _position += change;
    }

    public void ChangePointAndTangents(Vector3 positionChange, Vector3 tangentBackChange, Vector3 tangentFrontChange)
    {
        _position += positionChange;
        _tangentBack += tangentBackChange;
        _tangentFront += tangentFrontChange;

    }

    public void SetTangents(Vector3 tangentFront, Vector3 tangentBack)
    {
        _tangentFront = tangentFront;
        _tangentBack = tangentBack;
    }

    public ControlPoint(Vector3 position, Vector3 surfaceNormal, Vector3 tangentFront, Vector3 tangentBack, float radius = 0)
    {
        _position = position;
        _radius = radius;
        _surfaceNormal = surfaceNormal == Vector3.zero ? Vector3.up : surfaceNormal;

        _tangentFront = tangentFront;
        _tangentBack = tangentBack;
    }
}
