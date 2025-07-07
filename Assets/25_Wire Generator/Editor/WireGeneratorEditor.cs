using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WireGenerator))]
public class WireGeneratorEditor : Editor
{
    RaycastHit _mouseHit;
    Vector3 _previousMouseActionPoint;

    int _selectedControlIndex = -1;

    private void OnSceneGUI()
    {
        WireGenerator generator = target as WireGenerator;

        if (!Event.current.control)
        {
            DoControlPoints(generator);
        } 
        else
        {
            if (Event.current.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
            }

            if (Event.current.type == EventType.Repaint)
            {
                for (int i = 0; i < generator.ControlPoints.Length; i++)
                {
                    Vector3 controlPosition = generator.ControlPoints[i].GetOffsetPosition(generator.WireThickness);
                    Vector3 tangentBackPosition = controlPosition + generator.ControlPoints[i].TangentBack;
                    Vector3 tangentFrontPosition = controlPosition + generator.ControlPoints[i].TangentFront;

                    float scale = 0.3f;
                    Handles.color = new Color(1, 1, 1, 0.5f);
                    if (_selectedControlIndex == i)
                    {
                        scale = 0.6f;
                        Handles.color = new Color(1, 1, 1, 0.7f);
                    }

                    Handles.DotHandleCap(1010, controlPosition, Quaternion.identity, generator.WireThickness * scale, EventType.Repaint);
                    Handles.DotHandleCap(1010, tangentBackPosition, Quaternion.identity, generator.WireThickness * scale, EventType.Repaint);
                    Handles.DotHandleCap(1010, tangentFrontPosition, Quaternion.identity, generator.WireThickness * scale, EventType.Repaint);

                    Handles.DrawLine(controlPosition, tangentFrontPosition);
                    Handles.DrawLine(controlPosition, tangentBackPosition);
                }
            }

            DoAddControls(generator);
        }

        if (Event.current.keyCode == KeyCode.Backspace)
        {
            generator.RemoveControl(generator.ControlPoints[_selectedControlIndex]);
        }
    }

    ControlPoint _addedControlPoint;

    private void DoAddControls(WireGenerator generator)
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            _mouseHit = GetMouseHit();
            Undo.RecordObject(generator, "Added Control Points");
            
            _addedControlPoint = generator.AddControlPoint(_mouseHit.point, _mouseHit.normal, Vector3.zero, Vector3.zero);
            _previousMouseActionPoint = _mouseHit.point;

            _selectedControlIndex = generator.ControlPoints.Length - 1;

        }

        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
        {
            if (_addedControlPoint == null) { return; }
            _mouseHit = GetMouseHit();

            //if (Vector3.Distance(_previousMouseActionPoint, _mouseHit.point) < generator.PaintingResolution) { return; }

            Vector3 tangentFront = (_mouseHit.point - _addedControlPoint.Position);
            Vector3 tangentBack = tangentFront * -1;

            Undo.RecordObject(generator, "Modified Tangents");
            _addedControlPoint.SetTangents(tangentFront, tangentBack);
            EditorUtility.SetDirty(generator);
            HandleUtility.Repaint();

            //generator.AddControlPoint(_mouseHit.point, _mouseHit.normal, tangentFront, tangentBack);
            
            _previousMouseActionPoint = _mouseHit.point;

            generator.RegenerateMesh();
        }

        if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            _addedControlPoint = null;
        }
    }

    private void DoControlPoints(WireGenerator generator)
    {
        for (int i = 0; i < generator.ControlPoints.Length; i++)
        {
            Vector3 surfaceNormal = generator.ControlPoints[i].SurfaceNormal;
            Vector3 invertedSurfaceNormal = new Vector3(1 - surfaceNormal.x, 1 - surfaceNormal.y, 1 - surfaceNormal.z);

            Vector3 controlPosition = generator.ControlPoints[i].GetOffsetPosition(generator.WireThickness);
            Vector3 tangentBackPosition = controlPosition + generator.ControlPoints[i].TangentBack;
            Vector3 tangentFrontPosition = controlPosition + generator.ControlPoints[i].TangentFront;

            float scale = 0.5f;
            Handles.color = new Color(1, 1, 1, 0.75f);
            if (_selectedControlIndex == i)
            {
                scale = 0.8f;
                Handles.color = new Color(1, 1, 1, 1);
            }

            Vector3 positionChange = Handles.FreeMoveHandle(controlPosition, generator.WireThickness * scale, Vector3.zero, Handles.RectangleHandleCap) - controlPosition;
            Vector3 tangentBackChange = Handles.FreeMoveHandle(tangentBackPosition, generator.WireThickness * scale, Vector3.zero, Handles.RectangleHandleCap) - tangentBackPosition;
            Vector3 tangentFrontChange = Handles.FreeMoveHandle(tangentFrontPosition, generator.WireThickness * scale, Vector3.zero, Handles.RectangleHandleCap) - tangentFrontPosition;

            Handles.DrawLine(controlPosition, tangentFrontPosition);
            Handles.DrawLine(controlPosition, tangentBackPosition);
            
            if(positionChange != Vector3.zero || tangentBackChange != Vector3.zero || tangentFrontChange != Vector3.zero)
            {
                HandleUtility.Repaint();
                _selectedControlIndex = i;
                Undo.RecordObject(generator, "Control Edited");
                generator.RegenerateMesh();
            }

            if (Event.current.shift)
            {
                if (tangentBackChange != Vector3.zero)
                {
                    tangentFrontChange = Vector3.Scale(tangentBackChange, Vector3.one * -1);
                }

                if (tangentFrontChange != Vector3.zero)
                {
                    tangentBackChange = Vector3.Scale(tangentFrontChange, Vector3.one * -1);
                }
            }

            generator.ControlPoints[i].ChangePointAndTangents(positionChange, tangentBackChange, tangentFrontChange);
        }
    }

    private RaycastHit GetMouseHit()
    {
        Camera sceneCam = Camera.current;
        if (sceneCam == null) { return default; }

        Vector2 mousePos = Event.current.mousePosition;
        mousePos.y = Camera.current.pixelHeight - mousePos.y;
        Ray ray = sceneCam.ScreenPointToRay(mousePos);
        Physics.Raycast(ray, out RaycastHit hit);

        if (hit.collider == null) { return default; }
        return hit;
    }

    private enum EditingMode
    {
        None,
        Painting,
        Dragging,
        PerPoint
    }
}
