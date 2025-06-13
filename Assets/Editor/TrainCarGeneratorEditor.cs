using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrainCarGenerator))]
public class TrainCarGeneratorEditor : Editor
{
    int[] _handleIds;
    int _selectedHandleId = -1;
    Vector2 _mousePosPrev;

    private void OnSceneGUI()
    {
        TrainCarGenerator gen = (target as TrainCarGenerator);

        Vector3 lengthVector = Vector3.forward * gen.RequiredSegmentCount * gen.MinimumSegementLength;
        Vector3 startPosition = gen.transform.position;
        Vector3 endPosition = gen.transform.position + lengthVector;

        if (_handleIds == null || _handleIds.Length != gen.RequiredSegmentCount)
        {
            _handleIds = GetHandleIds("Train Car Handles", gen.RequiredSegmentCount);

            if (_selectedHandleId != -1)
            {
                _handleIds[^1] = _selectedHandleId;
            }
        }

        int nearestHandleId = HandleUtility.nearestControl;
        
        if (Event.current.type == EventType.Repaint)
        {
            Handles.color = Color.white;
            Handles.DrawAAPolyLine(6, startPosition, endPosition);

            for (int i = 0; i < _handleIds.Length; i++)
            {
                DrawHandleLerped(gen, startPosition, nearestHandleId, i, EventType.Repaint);
            }
        }

        if (Event.current.type == EventType.Layout)
        {
            for (int i = 0; i < _handleIds.Length; i++)
            {
                DrawHandleLerped(gen, startPosition, nearestHandleId, i, EventType.Layout);
            }
        }

        if (Event.current.type == EventType.MouseDown)
        {
            _selectedHandleId = HandleUtility.nearestControl;
            _mousePosPrev = Event.current.mousePosition;

            for (int i = 0; i < _handleIds.Length; i++)
            {
                if (_handleIds[i] != _selectedHandleId) { continue; }
                if (i == _handleIds.Length - 1) { continue; }

                SetNextValidSegmentIndex(gen, i);
                Regenerate(gen);
            }
        }

        if (Event.current.type == EventType.MouseDrag)
        {
            for (int i = 0; i < _handleIds.Length; i++)
            {
                if (_handleIds[i] != _selectedHandleId) { continue; }

                if (i == _handleIds.Length - 1)
                {
                    //is length handle
                    Vector3 position = startPosition + Vector3.forward * gen.TrainCarLength;

                    float change = HandleUtility.CalcLineTranslation(_mousePosPrev, Event.current.mousePosition, position, Vector3.forward);
                    gen.TrainCarLength += change;

                    if (gen.TrainCarLength < 1)
                    {
                        gen.TrainCarLength = 1;
                    }

                    Regenerate(gen);
                    continue;
                }
            }

            _mousePosPrev = Event.current.mousePosition;
        }

        HandleUtility.Repaint();
    }

    private void Regenerate(TrainCarGenerator gen)
    {
        gen.RegenerateCar();
        EditorUtility.SetDirty(gen.gameObject);
    }

    private void SetNextValidSegmentIndex(TrainCarGenerator gen, int i)
    {
        //is section handle
        TryResetIndecies(gen, i);

        int nextSegmentIndex = gen.SegmentLayout[i] + 1;
        int indexer = 0;
        while ((i + gen.DefinedSegments[(nextSegmentIndex % gen.DefinedSegments.Length)].Length) >= gen.RequiredSegmentCount && indexer < 100)
        {
            nextSegmentIndex++;
            indexer++;
        }

        nextSegmentIndex %= gen.DefinedSegments.Length;

        gen.SegmentLayout[i] = nextSegmentIndex;
        for (int j = 1; j < gen.DefinedSegments[nextSegmentIndex].Length; j++)
        {
            TryResetIndecies(gen, i + j);
            gen.SegmentLayout[i + j] = int.MinValue;
        }
    }

    private static void TryResetIndecies(TrainCarGenerator gen, int i)
    {
        for (int j = 1; j < gen.DefinedSegments[gen.SegmentLayout[i]].Length; j++)
        {
            if (!gen.SegmentLayout[i + j].Equals(int.MinValue))
            {
                TryResetIndecies(gen, i + j);
            }

            gen.SegmentLayout[i + j] = 0;
        }
    }

    private void DrawHandleLerped(TrainCarGenerator gen, Vector3 startPosition, int nearestHandleId, int i, EventType eventType)
    {
        Vector3 handlePosition = PositionFromHandleIndex(gen, startPosition, i);

        Color handleColor = Color.white;
        if (nearestHandleId == _handleIds[i]) { handleColor = Color.yellow; }
        Handles.color = handleColor;

        if (i == _handleIds.Length - 1)
        {
            Handles.ConeHandleCap(_handleIds[i], handlePosition, Quaternion.Euler(0, 0, 0), 0.25f, eventType);
        }
        else if (!gen.SegmentLayout[i].Equals(int.MinValue))
        {
            Handles.RectangleHandleCap(_handleIds[i], handlePosition, Quaternion.Euler(0, 90, 0), 0.1f, eventType);
        }
    }

    private static Vector3 PositionFromHandleIndex(TrainCarGenerator gen, Vector3 startPosition, int i)
    {
        Vector3 stepVector = Vector3.forward * gen.MinimumSegementLength * (i + 1);
        Vector3 handlePosition = startPosition + stepVector;
        return handlePosition;
    }

    private int[] GetHandleIds(string uniqueIdentfier, int requiredHandleCount)
    {
        int[] handles = new int[requiredHandleCount];
        for (int i = 0; i < handles.Length; i++)
        {
            handles[i] = GUIUtility.GetControlID(new GUIContent(uniqueIdentfier), FocusType.Passive);
        }
        return handles;
    }
}
