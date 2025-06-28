using System;
using UnityEngine;

[System.Serializable]
public class ControlPoint
{
    [SerializeField] private Vector3 _position;
    [SerializeField] private Vector3 _surfaceNormal;
    [SerializeField] private float _radius;

    public Vector3 Position => _position;
    public float Radius => _radius;
    public Vector3 SurfaceNormal => _surfaceNormal;

    public Vector3 GetOffsetPosition(float radius)
    {
        return _position + SurfaceNormal * (radius + Radius);
    }

    public void ChangePosition(float radius, Vector3 change)
    {
        _position += change;
    }

    public ControlPoint(Vector3 position, Vector3 surfaceNormal, float radius = 0)
    {
        _position = position;
        _radius = radius;
        _surfaceNormal = surfaceNormal == Vector3.zero ? Vector3.up : surfaceNormal;
    }
}
