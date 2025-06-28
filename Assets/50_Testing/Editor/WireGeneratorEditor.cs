using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WireGenerator))]
public class WireGeneratorEditor : Editor
{
    RaycastHit _mouseHit;
    EditingMode _currentEditMode;
    Vector3 _previousMouseActionPoint;

    Vector3 _prevMousePoint;

    int _lineHandleId = -1;
    int _selectedHandle = -1;
    int _closestPointIndex = -1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _currentEditMode = (EditingMode)EditorGUILayout.EnumPopup(_currentEditMode);
        //GUIStyle toggleStyle = GUI.skin.button;
        
        /*if (GUILayout.Toggle((_currentEditMode == EditingMode.Painting), new GUIContent("Paint"), toggleStyle))
        {
            _currentEditMode = EditingMode.None;
        }*/
    }

    private void OnSceneGUI()
    {
        if (_lineHandleId == -1)
        {
            _lineHandleId = GetHandleIDs("LineHandle", 1)[0];
        }

        WireGenerator generator = target as WireGenerator;



        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            _mouseHit = GetMouseHit();
            _previousMouseActionPoint = _mouseHit.point;
        }

        switch (_currentEditMode)
        {
            case EditingMode.Dragging:
                DoDragHandles(generator);
                break;
        }

        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
        {
            _mouseHit = GetMouseHit();
            switch (_currentEditMode)
            {
                case EditingMode.Painting:
                    if (Vector3.Distance(_previousMouseActionPoint, _mouseHit.point) < generator.PaintingResolution) { break; }
                    generator.AddControlPoint(_mouseHit.point, _mouseHit.normal);
                    _previousMouseActionPoint = _mouseHit.point;
                    EditorUtility.SetDirty(generator);
                    break;

                case EditingMode.Dragging:

                    break;

                default:
                    break;
            }
            HandleUtility.Repaint();
        }

        if (Event.current.type == EventType.Repaint)
        {
            Handles.DrawWireDisc(_mouseHit.point, _mouseHit.normal, 0.15f);
        }

        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
        }
    }

    private void DoDragHandles(WireGenerator generator)
    {
        Vector3 positionChange = Vector3.zero;

        if (Event.current.type == EventType.Layout)
        {
            float minimumFoundDistance = float.PositiveInfinity;
            for (int i = 0; i < generator.ControlPoints.Length; i++)
            {
                Vector3 position = generator.ControlPoints[i].GetOffsetPosition(generator.WireThickness);
                
/*                if (_closestPointIndex == i)
                {
                    Vector3 newPosition = Handles.FreeMoveHandle(position, generator.WireThickness, Vector3.zero, Handles.RectangleHandleCap);
                    positionChange = newPosition - position;
                }*/
                
                float distance = Vector2.Distance(HandleUtility.WorldToGUIPoint(position), Event.current.mousePosition);
                if (distance < minimumFoundDistance)
                {
                    minimumFoundDistance = distance;

                    if (_selectedHandle == -1)
                    {
                        _closestPointIndex = i;
                    }
                }
                //HandleUtility.AddControl(_lineHandleId, minimumFoundDistance * 0.1f);
            }
        }

        if (Event.current.type == EventType.Repaint)
        {
            for (int i = 0; i < generator.ControlPoints.Length - 1; i++)
            {
                Vector3 position = generator.ControlPoints[i].GetOffsetPosition(generator.WireThickness);

/*                if (_closestPointIndex == i)
                {
                    Vector3 newPosition = Handles.FreeMoveHandle(position, generator.WireThickness, Vector3.zero, Handles.RectangleHandleCap);
                }*/

                Vector3 positionNext = generator.ControlPoints[i + 1].GetOffsetPosition(generator.WireThickness);
                float distanceFromMouse = Vector2.Distance(HandleUtility.WorldToGUIPoint((position + positionNext) * 0.5f), Event.current.mousePosition);
                float distanceFromCamera = Vector3.Distance((position + positionNext) * 0.5f, Camera.current.transform.position);
                //Handles.color = new Color(1, 1, 1, 1f / (0.1f * minimumFoundDistance));
                Handles.color = new Color(1, 1, 1, 1.0f / (0.01f * distanceFromCamera * distanceFromMouse));
                Handles.DrawLine(position, positionNext, 1.5f);
            }
        }

        for (int i = 0; i < generator.ControlPoints.Length - 1; i++)
        {
            Vector3 position = generator.ControlPoints[i].GetOffsetPosition(generator.WireThickness);

            if (_closestPointIndex == i)
            {
                Vector3 newPosition = Handles.FreeMoveHandle(position, generator.WireThickness, Vector3.zero, Handles.RectangleHandleCap);
                positionChange = newPosition - position;
            }
        }

        int selectionSize = 2;


        for (int i = 0; i < generator.ControlPoints.Length; i++)
        {
            int difference = Mathf.Abs(i - _closestPointIndex);
            if (difference <= selectionSize)
            {
                float mult = 1 - (float)difference / (selectionSize + 1);
                mult = Mathf.Clamp(mult, 0, float.PositiveInfinity);

                generator.ControlPoints[i].ChangePosition(generator.WireThickness, positionChange * mult);
            }
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

    public static int[] GetHandleIDs(string uniqueIdentifier, int numberOfHandles)
    {
        int[] handles = new int[numberOfHandles];
        for (int i = 0; i < numberOfHandles; i++)
        {
            handles[i] = GUIUtility.GetControlID(new GUIContent(uniqueIdentifier), FocusType.Passive);
        }
        return handles;
    }

    private enum EditingMode
    {
        None,
        Painting,
        Dragging,
        PerPoint
    }
}
