using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Mediapipe.CopyCalculatorOptions.Types;

[ExecuteInEditMode]
public class WireGenerator : MonoBehaviour
{
    [SerializeField, Min(3)] private int _wireResolution = 6;
    [SerializeField] private ControlPoint[] _controlPoints;
    [SerializeField] private Material _material;
    [SerializeField, Range(0.001f, 0.5f)] private float _wireThickness;
    [SerializeField, Min(0f)] private float _paintingResolution = 0.05f;
    //[SerializeField, Min(0f)] private float _surfaceOffsetPercentage = 1;

    private Mesh _mesh;

    public float PaintingResolution => _paintingResolution;
    public float WireThickness => _wireThickness;
    public int WireResolution => _wireResolution;
    public ControlPoint[] ControlPoints => _controlPoints;

    private void Update()
    {
        if (_mesh == null)
        {
        }
        _mesh = GenerateWireMesh(_controlPoints);
        Graphics.RenderMesh(new RenderParams(_material), _mesh, 0, transform.localToWorldMatrix);
    }

    List<Vector3> points;

    private Mesh GenerateWireMesh(ControlPoint[] controlPoints)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int i = 0; i < controlPoints.Length; i++)
        {
            ControlPoint prevControl, control, nextControl;
            prevControl = (i - 1 >= 0 ? controlPoints[i - 1] : null);
            nextControl = (i + 1 < controlPoints.Length ? controlPoints[i + 1] : null);
            control = controlPoints[i];

            vertices.AddRange(GetVerticesFromControlPoint(prevControl, control, nextControl));
        }

        for (int i = 0; i < vertices.Count - WireResolution; i++)
        {

            //add triangle points
            //tri 1
            triangles.Add(i + 1);
            if (i % WireResolution != WireResolution - 1) { triangles.Add(i + WireResolution + 1); }
            triangles.Add(i);
            if (i % WireResolution == WireResolution - 1) { triangles.Add(i - (WireResolution - 1)); } 

            //tri2
            if (i % WireResolution != WireResolution - 1) { triangles.Add(i + WireResolution + 1); }
            triangles.Add(i + WireResolution);
            triangles.Add(i);
            if (i % WireResolution == WireResolution - 1) { triangles.Add(i + 1); }
        }

        points = vertices;

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);

        mesh.RecalculateNormals();

        return mesh;
    }

    private List<Vector3> GetVerticesFromControlPoint(ControlPoint prevControl, ControlPoint control, ControlPoint nextControl)
    {
        List<Vector3> vertices = new List<Vector3>();

        Vector3 dirFromPrev = Vector3.zero;
        Vector3 dirToNext = Vector3.zero;
        if (prevControl != null) { dirFromPrev = (control.GetOffsetPosition(_wireThickness) - prevControl.GetOffsetPosition(_wireThickness)).normalized; }
        if (nextControl != null) { dirToNext = (nextControl.GetOffsetPosition(_wireThickness) - control.GetOffsetPosition(_wireThickness)).normalized; }
        Vector3 controlForward = (dirFromPrev + dirToNext).normalized;
        Vector3 controlLeft = Vector3.Cross(controlForward, control.SurfaceNormal).normalized;

        float anglePerPoint = 360f / WireResolution;
        for (int i = 0; i < WireResolution; i++)
        {
            //calculate and add verticies
            vertices.Add(control.GetOffsetPosition(_wireThickness) + Quaternion.AngleAxis(anglePerPoint * i, controlForward) * controlLeft * (_wireThickness + control.Radius));
        }
        return vertices;
    }

    private void OnDrawGizmos()
    {
        /*for (int i = 0; i < points.Count; i++)
        {
            Gizmos.DrawSphere(points[i], 0.05f);
        }*/
    }

    public void AddControlPoint(Vector3 point, Vector3 normal)
    {
        List<ControlPoint> tempControlPoints = new List<ControlPoint>(_controlPoints);
        tempControlPoints.Add(new ControlPoint(point, normal));
        _controlPoints = tempControlPoints.ToArray();
    }
}
