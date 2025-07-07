using System;
using System.Collections.Generic;
using UnityEngine;
using static Mediapipe.ImageFormat.Types;

[ExecuteInEditMode]
[AddComponentMenu("Custom/Wire Generator")]
public class WireGenerator : MonoBehaviour
{
    [SerializeField, Min(3)] private int _wireResolution = 6;
    [SerializeField, Min(3)] private int _perCurveResolution = 30;
    [SerializeField] private ControlPoint[] _controlPoints;
    [SerializeField, Range(0.001f, 0.5f)] private float _wireThickness;

    private Mesh _mesh;

    public float WireThickness => _wireThickness;
    public int WireResolution => _wireResolution;
    public ControlPoint[] ControlPoints => _controlPoints;

    public Material Material;

    private void OnValidate()
    {
        RegenerateMesh();
    }

    private void Update()
    {
        if (_mesh == null)
        {
            RegenerateMesh();
        }
        Graphics.RenderMesh(new RenderParams(Material), _mesh, 0, transform.localToWorldMatrix);
    }

    public void RegenerateMesh()
    {
        _mesh = GenerateWireMesh(_controlPoints);
    }

    private Mesh GenerateWireMesh(ControlPoint[] controlPoints)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        vertices.AddRange(GetVerticesFromControls(controlPoints));

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

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);

        mesh.RecalculateNormals();

        return mesh;
    }

    private List<Vector3> GetVerticesFromControls(ControlPoint[] controlPoints)
    {
        List<List<Vector3>> points = new List<List<Vector3>>();
        List<Vector3> vertices = new List<Vector3>();

        for (int i = 0; i < controlPoints.Length-1; i++)
        {
            points.Add(GetPointsOnSegment(controlPoints[i], controlPoints[i + 1]));
        }

        for (int i = 0; i < points.Count; i++)
        {
            for (int j = 0; j < points[i].Count; j++)
            {
                Vector3 pointCurrent = points[i][j], pointPrev = default, pointNext = default;
                
                if (j != 0)
                {
                    pointPrev = points[i][j - 1];
                } 
                else if (i != 0)
                {
                    pointPrev = points[i - 1][^1];
                }

                if (j != points[i].Count - 1)
                {
                    pointNext = points[i][j + 1];
                } 
                else if (i != points.Count - 1)
                {
                    pointNext = points[i + 1][0];
                }

                vertices.AddRange(GetVerticesFromPoints(pointPrev, pointCurrent, pointNext, controlPoints[i]));
            }
        }

        return vertices;
    }

    private List<Vector3> GetVerticesFromPoints(Vector3 pointPrev, Vector3 pointCurrent, Vector3 pointNext, ControlPoint control)
    {
        List<Vector3> vertices = new List<Vector3>();

        Vector3 dirFromPrev = Vector3.zero;
        Vector3 dirToNext = Vector3.zero;
        if (pointPrev != null) { dirFromPrev = (pointCurrent - pointPrev).normalized; }
        if (pointNext != null) { dirToNext = (pointNext - pointCurrent).normalized; }
        Vector3 controlForward = (dirFromPrev + dirToNext).normalized;
        Vector3 controlLeft = Vector3.Cross(controlForward, control.SurfaceNormal).normalized;

        float anglePerPoint = 360f / WireResolution;
        for (int i = 0; i < WireResolution; i++)
        {
            //calculate and add verticies
            vertices.Add(pointCurrent + Quaternion.AngleAxis(anglePerPoint * i, controlForward) * controlLeft * (_wireThickness + control.Radius));
        }
        return vertices;
    }

    public List<Vector3> GetPointsOnSegment(ControlPoint controlFirst, ControlPoint controlLast)
    {
        List<Vector3> points = new List<Vector3>();
        float distance = Vector3.Distance(controlFirst.GetOffsetPosition(WireThickness), controlLast.GetOffsetPosition(WireThickness));

        int numberOfPoints = Mathf.RoundToInt(_perCurveResolution * distance);

        for (int i = 0; i < numberOfPoints; i++)
        {
            points.Add(GetPointOnBezierCurve(new Vector3[] { controlFirst.GetOffsetPosition(WireThickness), controlFirst.GetOffsetPosition(WireThickness) + controlFirst.TangentFront, controlLast.GetOffsetPosition(WireThickness) + controlLast.TangentBack, controlLast.GetOffsetPosition(WireThickness) }, i/(float)numberOfPoints));
        }

        return points;
    }

    private Vector3 GetPointOnBezierCurve(Vector3[] curvePoints, float t)
    {
        float tSquared = t * t;
        float tCubed = tSquared * t;

        Vector3 point =
            curvePoints[0] * (-tCubed + 3 * tSquared - 3 * t + 1) +
            curvePoints[1] * (3 * tCubed - 6 * tSquared + 3 * t) +
            curvePoints[2] * (-3 * tCubed + 3 * tSquared) +
            curvePoints[3] * tCubed;
        return point;
    }

    public ControlPoint AddControlPoint(Vector3 point, Vector3 normal, Vector3 tangentFront, Vector3 tangentBack)
    {
        List<ControlPoint> tempControlPoints = new List<ControlPoint>(_controlPoints);
        ControlPoint controlPoint = new ControlPoint(point, normal, tangentFront, tangentBack);
        tempControlPoints.Add(controlPoint);
        _controlPoints = tempControlPoints.ToArray();

        RegenerateMesh();

        return controlPoint;
    }

    public void RemoveControl(ControlPoint controlPoint)
    {
        List<ControlPoint> tempControlPoints = new List<ControlPoint>(_controlPoints);
        tempControlPoints.Remove(controlPoint);
        _controlPoints = tempControlPoints.ToArray();

        RegenerateMesh();
    }
}
