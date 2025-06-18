using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GraphEditorWindow : EditorWindow
{
    Graph _graph;
    GraphEditorWindow _window;
    Vector2 _mousePosition;
    int _controlId = -1;
    int _controlIdCounter = 0;

    public void CreateWindow(Graph graph)
    {
        _graph = graph;

        _window = GetWindow<GraphEditorWindow>();
        _window.titleContent = new GUIContent("Event Graph Editor");
    }

    private void OnGUI()
    {
        _controlIdCounter = 0;

        for (int i = 0; i < _graph.Nodes.Length; i++)
        {
            RenderNode(_graph.Nodes[i]);
        }

        if (Event.current.type == EventType.MouseUp)
        {
            _controlId = -1;
        }
    }

    private void RenderNode(GraphNode graphNode)
    {
        EditorGUI.indentLevel = 0;

        int cursorHeight = 0;
        float propertyWidth = 300;
        float propertyHeight = EditorGUIUtility.singleLineHeight;
        
        float nodeHeight = propertyHeight * (graphNode.NodeComponents.Length + 1);
        Rect nodeRect = new Rect(graphNode.Position, new Vector2(propertyWidth, nodeHeight));
        EditorGUI.DrawRect(nodeRect, new Color(0.2f, 0.2f, 0.2f, 1));

        if (ObjectWasDraggedIn(nodeRect, out GameObject draggedInObject))
        {
            HandleObjectDraggedIn(graphNode, draggedInObject);
        }

        Rect headerRect = ReserveRect(graphNode, nodeRect, ref cursorHeight);
        DrawNodeHeader(headerRect, graphNode);
        graphNode.ChangePosition(CalcRectDragged(headerRect));
        //EditorGUI.DrawRect(nodeRect, new Color(0.3f, 0.3f, 0.3f, 1));

        for (int i = 0; i < graphNode.NodeComponents.Length; i++)
        {
            if (graphNode.Type == GraphNode.NodeType.CONDITION) { break; }

            Rect propertyRect = ReserveRect(graphNode, nodeRect, ref cursorHeight);
            EditorGUI.DrawRect(propertyRect, new Color(0.3f, 0.3f, 0.3f, 1));

            string effectName = $"({graphNode.NodeComponents[i].gameObject.name}) {((IGraphEffectable)graphNode.NodeComponents[i]).GetEffectName()}";
            EditorGUI.LabelField(propertyRect, new GUIContent(effectName));
            continue;

       /*     IGraphConditional conditional = (IGraphConditional)graphNode.NodeComponents[i];
            string name = $"({graphNode.NodeComponents[i].gameObject.name}) {conditional.GetConditionName()}";
            EditorGUI.LabelField(propertyRect, new GUIContent(name));*/

            //Type[] argTypes = conditional.GetArgTypes();
        }

        for (int i = 0; i < graphNode.conditions.Count; i++)
        {
            EditorGUI.indentLevel = 0;
            if (graphNode.Type == GraphNode.NodeType.EFFECT) { break; }

            Condition conditional = graphNode.conditions[i];
            SerializedObject obj = new SerializedObject(conditional);

            Rect nameRect = ReserveRect(graphNode, nodeRect, ref cursorHeight);
            string name = $"({graphNode.conditions[i].ReferencedObject.name}) {conditional.GetConditionName()}";
            EditorGUI.LabelField(nameRect, new GUIContent(name));

            SerializedProperty iterator = obj.GetIterator();
            //iterator.Next(true);
            EditorGUI.indentLevel = 1;

            while (iterator.NextVisible(true))
            {
                Rect propertyRect = ReserveRect(graphNode, nodeRect, ref cursorHeight);
                EditorGUI.PropertyField(propertyRect, iterator);
            }

            obj.ApplyModifiedProperties();
        }
    }

    private void HandleObjectDraggedIn(GraphNode graphNode, GameObject draggedInObject)
    {
        List<Component> selectedComponents = new List<Component>();
        switch (graphNode.Type)
        {
            case GraphNode.NodeType.EFFECT:
                IGraphEffectable[] effectable = draggedInObject.GetComponents<IGraphEffectable>();
                for (int i = 0; i < effectable.Length; i++)
                {
                    selectedComponents.Add((Component)effectable[i]);
                }
                break;

            case GraphNode.NodeType.CONDITION:
                /*                IGraphConditional[] conditional = draggedInObject.GetComponents<IGraphConditional>();
                                for (int i = 0; i < conditional.Length; i++)
                                {
                                    selectedComponents.Add((Component)conditional[i]);
                                }*/

                TestCondition condition = CreateInstance<TestCondition>();
                condition.Initialize(draggedInObject);
                graphNode.conditions.Add(condition);
                break;

            default:
                break;
        }

        if (selectedComponents.Count <= 0) { return; }

        graphNode.AddComponent(selectedComponents[0]);
    }

    private bool ObjectWasDraggedIn(Rect rect, out GameObject obj)
    {
        obj = null;
        if (!rect.Contains(Event.current.mousePosition)) { return false; }

        DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
        if (Event.current.type == EventType.DragExited)
        {
            if (DragAndDrop.objectReferences.Length == 1)
            {
                obj = (GameObject)DragAndDrop.objectReferences[0];
                return true;
            }
        }
        return false;
    }

    private Vector2 CalcRectDragged(Rect headerRect)
    {
        int controlId = GetNextControlId();

        if (Event.current.button != 0) { return Vector2.zero; }

        if (controlId != _controlId) 
        {
            if (Event.current.type == EventType.MouseDown && headerRect.Contains(Event.current.mousePosition))
            {
                _controlId = controlId;
                _mousePosition = Event.current.mousePosition;
            }
            return Vector2.zero; 
        }

        if (Event.current.type == EventType.MouseDrag)
        {
            HandleUtility.Repaint();
            Vector2 change = Event.current.mousePosition - _mousePosition;
            _mousePosition = Event.current.mousePosition;
            return change;
        }
        return Vector2.zero;
    }

    private int GetNextControlId()
    {
        return _controlIdCounter++;
    }

    private void DrawNodeHeader(Rect rect, GraphNode graphNode)
    {
        EditorGUI.LabelField(rect, graphNode.Type.ToString());
    }

    private Rect ReserveRect(GraphNode graphNode, Rect nodeRect, ref int cursorHeight)
    {
        float indent = EditorGUI.indentLevel * 5;
        Rect returnRect = new Rect
            (
            new Vector2(indent, EditorGUIUtility.singleLineHeight * cursorHeight) + graphNode.Position, 
            new Vector2(nodeRect.width - indent, EditorGUIUtility.singleLineHeight)
            );

        cursorHeight++;
        return returnRect;
    }
}
