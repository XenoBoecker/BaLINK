using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class GraphEditorWindow : EditorWindow
{
    private const int CONNECTOR_RECT_SIZE = 15;
    private const int ELEMENT_PADDING = 8;
    private float PROPERTY_HEIGHT => EditorGUIUtility.singleLineHeight;

    private static Color BODY_COLOR = new Color(0.3f, 0.3f, 0.3f, 1.0f);
    private static Color OUTLINE_COLOR = new Color(0.15f, 0.15f, 0.15f, 1.0f);
    private static Color SELECTED_COLOR = new Color(0.5f, 0.5f, 0.5f, 1.0f);

    Type[] _allnodeElementTypes => GetAllSubclassesOf(typeof(NodeElement)).ToArray();
    SelectionMenu<NodeType> _newNodeSelectionMenu;

    int _newElementTargetNodeId = -1;
    GameObject _draggedInObject = null;
    SelectionMenu<Type> _newElementSelectionMenu;

    Graph _graph;
    GraphEditorWindow _window;
    Vector2 _mousePosition;
    int _controlId = -1;
    int _controlIdCounter = 0;

    int _sourceNodeId = -1;
    int _targetNodeId = -1;

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

        if (_newElementTargetNodeId != -1 && _newElementSelectionMenu != null && _newElementSelectionMenu.Options.Length != 0)
        {
            SelectionResult result = DrawSelectionMenu(_newElementSelectionMenu, out object selection);
            switch (result)
            {
                case SelectionResult.Failed:
                    _newElementSelectionMenu = null;
                    _draggedInObject = null;
                    _newElementTargetNodeId = -1;
                    break;
                case SelectionResult.Succeeded:
                    ElementSelectionMade((Type)selection);
                    break;
            }
        }

        if (_newNodeSelectionMenu != null && _newNodeSelectionMenu.Options.Length != 0)
        {
            SelectionResult result = DrawSelectionMenu(_newNodeSelectionMenu, out object selection);
            switch (result)
            {
                case SelectionResult.Failed:
                    _newNodeSelectionMenu = null;
                    break;
                case SelectionResult.Succeeded:
                    NodeSelectionMade((NodeType)selection);
                    break;
            }
        }

        if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            _controlId = -1;
        }

        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            _newNodeSelectionMenu = new SelectionMenu<NodeType>(Event.current.mousePosition, new Vector2(200, PROPERTY_HEIGHT), new NodeType[] { NodeType.Condition, NodeType.Effect });
            HandleUtility.Repaint();
        }
    }

    private void NodeSelectionMade(NodeType selection)
    {
        _graph.AddNewNode(selection, _newNodeSelectionMenu.Position);

        _newNodeSelectionMenu = null;
    }

    void ElementSelectionMade(Type selectedType)
    {
        NodeElement condition = (NodeElement)CreateInstance(selectedType);
        condition.Initialize(_draggedInObject);
        
        if (_graph.TryGetNodeFromId(_newElementTargetNodeId ,out GraphNode node)) 
        {
            node.AddElement(condition);
        } 
        else
        {
            throw new Exception($"Could not add element because no node could be found with a matching id:({_newElementTargetNodeId})");
        }

        _newElementTargetNodeId = -1;
        _newElementSelectionMenu = null;
        _draggedInObject = null;
    }

    private SelectionResult DrawSelectionMenu<T>(SelectionMenu<T> menu, out object selection)
    {
        selection = null;

        for (int i = 0; i < menu.Options.Length; i++)
        {
            Rect optionRect = new Rect
                (menu.Position + new Vector2(0, i * menu.SingleSelectionSize.y),
                menu.SingleSelectionSize);

            Handles.DrawSolidRectangleWithOutline(optionRect, BODY_COLOR, OUTLINE_COLOR);
            EditorGUI.LabelField(optionRect, menu.Options[i].ToString());

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && optionRect.Contains(Event.current.mousePosition))
            {
                selection = menu.Options[i];
                HandleUtility.Repaint();
                return SelectionResult.Succeeded;
            }
        }

        if (Event.current.type == EventType.MouseDown)
        {
            HandleUtility.Repaint();
            return SelectionResult.Failed;
        }

        return SelectionResult.None;
    }

    private void RenderNode(GraphNode node)
    {
        EditorGUI.indentLevel = 0;

        int cursorHeight = 0;
        float propertyWidth = 300;
        
        float nodeHeight = PROPERTY_HEIGHT * (GetNodeHeight(node) + 1) + ELEMENT_PADDING * 0.5f;
        Rect nodeRect = new Rect(node.Position, new Vector2(propertyWidth, nodeHeight));

        Handles.DrawSolidRectangleWithOutline(nodeRect, BODY_COLOR, OUTLINE_COLOR);
        //EditorGUI.DrawRect(nodeRect, new Color(0.2f, 0.2f, 0.2f, 1));

        if (ObjectWasDraggedIn(nodeRect, out GameObject draggedInObject))
        {
            HandleObjectDraggedIn(node, draggedInObject);
        }

        Rect headerRect = ReserveRect(node, nodeRect, ref cursorHeight);
        DrawNodeHeader(headerRect, node);
        node.ChangePosition(CalcRectDragged(headerRect));

        Rect connectorInRect = new Rect(nodeRect.position + new Vector2(-CONNECTOR_RECT_SIZE, 0), Vector2.one * CONNECTOR_RECT_SIZE);
        EditorGUI.DrawRect(connectorInRect, new Color(0.2f, 1.0f, 1.0f, 1));

        Rect connectorOutRect = new Rect(nodeRect.position + new Vector2(propertyWidth, 0), Vector2.one * CONNECTOR_RECT_SIZE);
        EditorGUI.DrawRect(connectorOutRect, new Color(1.0f, 1.0f, 0.2f, 1));

        HandleAddNodeConnection(node, connectorInRect, NodeConnectionType.In);
        HandleAddNodeConnection(node, connectorOutRect, NodeConnectionType.Out);

        DrawNodeConnection(node, nodeRect);

        for (int i = 0; i < node.Elements.Length; i++)
        {
            DrawElement(node, ref cursorHeight, nodeRect, i);
        }
    }

    private void DrawElement(GraphNode node, ref int cursorHeight, Rect nodeRect, int i)
    {
        EditorGUI.indentLevel = 0;

        NodeElement element = node.Elements[i];
        SerializedObject serializedObject = new SerializedObject(element);

        Rect nameRect = ReserveRect(node, nodeRect, ref cursorHeight, ELEMENT_PADDING);

        Rect elementRect = new Rect(nameRect.position, new Vector2(nameRect.width, GetElementHeight(element) * PROPERTY_HEIGHT));
        Handles.DrawSolidRectangleWithOutline(elementRect, BODY_COLOR, OUTLINE_COLOR);

        string name = $"({node.Elements[i].ReferencedObject.name}) {element.GetName()}";
        node.Elements[i].SetUnfolded(EditorGUI.Foldout(nameRect, node.Elements[i].IsUnfolded, new GUIContent(name)));

        if (!node.Elements[i].IsUnfolded) { return; }

        SerializedProperty iterator = serializedObject.GetIterator();

        //skips to the next variable (skips the "script" field)
        iterator.NextVisible(true);

        EditorGUI.indentLevel = 1;

        while (iterator.NextVisible(true))
        {
            Rect propertyRect = ReserveRect(node, nodeRect, ref cursorHeight, ELEMENT_PADDING);
            EditorGUI.PropertyField(propertyRect, iterator);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private int GetNodeHeight(GraphNode node)
    {
        int heigth = 0;

        foreach (NodeElement element in node.Elements)
        {
            heigth += GetElementHeight(element);
        }

        return heigth;
    }

    private int GetElementHeight(NodeElement element)
    {
        int heigth = 1;
        SerializedObject serializedObject = new SerializedObject(element);

        //skips to the next variable (skips the "script" field)
        if (!element.IsUnfolded) { return heigth; }

        SerializedProperty iterator = serializedObject.GetIterator();

        iterator.NextVisible(true);
        while (iterator.NextVisible(true))
        {
            heigth++;
        }

        return heigth;
    }

    private void DrawNodeConnection(GraphNode node, Rect nodeRect)
    {
        if (_graph.TryGetNodeFromId(node.NextNodeId, out GraphNode foundNode))
        {
            Handles.DrawAAPolyLine(4    , node.Position + new Vector2(nodeRect.width, 0) + Vector2.one * 0.5f * CONNECTOR_RECT_SIZE, foundNode.Position + new Vector2(-1, 1) * 0.5f * CONNECTOR_RECT_SIZE);
        }
    }

    private void HandleAddNodeConnection(GraphNode node, Rect controlRect, NodeConnectionType type)
    {
        if (!controlRect.Contains(Event.current.mousePosition)) { return; }

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && controlRect.Contains(Event.current.mousePosition))
        {
            SetSourceOrTargetNode(node, type);
            return;
        }

        if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            SetSourceOrTargetNode(node, type);

            if (_graph.TryGetNodeFromId(_sourceNodeId, out GraphNode sourceNode) &&
                _graph.TryGetNodeFromId(_targetNodeId, out GraphNode targetNode) &&
                sourceNode != targetNode)
            {
                sourceNode.SetNextNodeId(_targetNodeId);
                HandleUtility.Repaint();
            }
            else
            {
                _sourceNodeId = -1;
                _targetNodeId = -1;
            }
        }
    }

    private void SetSourceOrTargetNode(GraphNode node, NodeConnectionType type)
    {
        switch (type)
        {
            case NodeConnectionType.In:
                _targetNodeId = node.Id;
                break;
            case NodeConnectionType.Out:
                _sourceNodeId = node.Id;
                break;
        }
    }

    private void HandleObjectDraggedIn(GraphNode node, GameObject draggedInObject)
    {
        Type[] allNodeElementsTypes = _allnodeElementTypes;
        List<Type> selectedNodeElementTypes = new List<Type>();
        for (int i = 0; i < allNodeElementsTypes.Length; i++)
        {
            NodeElementAttribute attribute = (NodeElementAttribute)Attribute.GetCustomAttribute(allNodeElementsTypes[i], typeof(NodeElementAttribute));
            
            if (attribute == null)
            {
                Debug.LogWarning($"Element({allNodeElementsTypes[i].Name}) does not have a NodeElementAttribute attached");
                continue;
            }

            if (attribute.ElementType == node.Type)
            {
                selectedNodeElementTypes.Add(allNodeElementsTypes[i]);
            }
        }

        _draggedInObject = draggedInObject;
        _newElementTargetNodeId = node.Id;
        _newElementSelectionMenu = new SelectionMenu<Type>(Event.current.mousePosition, new Vector2(200, PROPERTY_HEIGHT), selectedNodeElementTypes.ToArray());
        HandleUtility.Repaint();
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
                EditorUtility.SetDirty(_graph);
                return true;
            }
        }
        return false;
    }

    private Vector2 CalcRectDragged(Rect rect)
    {
        int controlId = GetNextControlId();

        if (Event.current.button != 0) { return Vector2.zero; }

        if (controlId != _controlId) 
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains(Event.current.mousePosition))
            {
                _controlId = controlId;
                _mousePosition = Event.current.mousePosition;
            }
            return Vector2.zero; 
        }

        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
        {
            HandleUtility.Repaint();
            EditorUtility.SetDirty(_graph);

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

    private void DrawNodeHeader(Rect rect, GraphNode node)
    {
        GUIStyle headerStyle = new GUIStyle();
        headerStyle.alignment = TextAnchor.MiddleCenter;
        headerStyle.normal.textColor = Color.white;

        EditorGUI.LabelField(rect, node.Type.ToString(), headerStyle);
    }

    private Rect ReserveRect(GraphNode node, Rect nodeRect, ref int cursorHeight, int horizontalPadding = 0)
    {
        float indent = EditorGUI.indentLevel * 5;
        Rect returnRect = new Rect
            (
            new Vector2(indent, PROPERTY_HEIGHT * cursorHeight) + node.Position + new Vector2(horizontalPadding * 0.5f, 0),
            new Vector2(nodeRect.width - indent, PROPERTY_HEIGHT) - new Vector2(horizontalPadding, 0)
            );

        cursorHeight++;
        return returnRect;
    }

    public static Type[] GetAllSubclassesOf(Type baseType)
    {
        return Assembly.GetAssembly(baseType).GetTypes().Where(type => type.IsSubclassOf(baseType)).ToArray();
    }

    private enum NodeConnectionType
    {
        In,
        Out
    }

    private enum SelectionResult
    {
        None,
        Failed,
        Succeeded,
    }
}
