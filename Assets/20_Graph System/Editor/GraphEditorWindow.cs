using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GraphEditorWindow : EditorWindow
{
    private const int CONNECTOR_RECT_SIZE = 18;
    private const int ELEMENT_PADDING = 8;
    private float PROPERTY_HEIGHT => EditorGUIUtility.singleLineHeight;
    private float PROPERTY_WIDTH => 300;

    private static Color BODY_COLOR = new Color(0.3f, 0.3f, 0.3f, 1.0f);
    private static Color OUTLINE_COLOR = new Color(0.15f, 0.15f, 0.15f, 1.0f);
    private static Color SELECTED_COLOR = new Color(0.5f, 0.5f, 0.5f, 1.0f);

    static Graph _graph;
    static GraphEditorWindow _window;
    
    Type[] _allnodeElementTypes => GetAllSubclassesOf(typeof(NodeElement)).ToArray();
    SelectionMenu<NodeType> _newNodeSelectionMenu;

    int _newElementTargetNodeId = -1;
    GameObject _draggedInObject = null;
    SelectionMenu<Type> _newElementSelectionMenu;

    Vector2 _mousePosition;
    bool _isDraggingNode = false;

    int _selectedNodeId = -1;
    bool _nodeSelectedThisFrame = false;

    int _sourceNodeId = -1;
    int _targetNodeId = -1;

    bool _isDraggingWindow = false;

    public static void CreateWindow(Graph graph)
    {
        _graph = graph;
        
        if (_window == null)
        {
            FocusWindowIfItsOpen(typeof(GraphEditorWindow));
            if(focusedWindow.GetType() == typeof(GraphEditorWindow))
            {
                _window = (GraphEditorWindow)focusedWindow;
            } 
            else
            {
                _window = GetWindow<GraphEditorWindow>();
            }
        }

        _window.Focus();
        _window.titleContent = new GUIContent($"({graph.gameObject.name}) Graph Editor");
    }

    private void OnGUI()
    {
        if (_graph == null) { return; }

        _nodeSelectedThisFrame = false;

        for (int i = 0; i < _graph.Nodes.Length; i++)
        {
            DrawNode(_graph.Nodes[i]);
        }

        HandleAddNode();
        HandleAddElementToNode();

        if (Event.current.type == EventType.MouseUp)
        {
            _isDraggingNode = false;
            _isDraggingWindow = false;
        }

        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            if (_selectedNodeId != -1)
            {
                //try add element to node
                /*_newElementTargetNodeId = _selectedNodeId;
                _newElementSelectionMenu = new SelectionMenu<Type>(Event.current.mousePosition);*/
            }
            else
            {
                _newNodeSelectionMenu = new SelectionMenu<NodeType>(Event.current.mousePosition,
                    new NodeType[] { NodeType.Entry, NodeType.Condition, NodeType.Effect });
                HandleUtility.Repaint();
            }
        }

        if (Event.current.type == EventType.MouseDown && Event.current.button == 2 && _window.IsFocused())
        {
            //pan window with middle mouse
            _mousePosition = Event.current.mousePosition;
            _isDraggingWindow = true;
        }

        if (Event.current.type == EventType.MouseDrag && Event.current.button == 2 && _isDraggingWindow)
        {
            //pan window with middle mouse
            HandleUtility.Repaint();
            EditorUtility.SetDirty(_graph);
            _graph.ChangeGraphCenter(GetMouseDragChange());
        }

        if (Event.current.type == EventType.MouseDown)
        {
            if (!_nodeSelectedThisFrame)
            {
                _selectedNodeId = -1;
                HandleUtility.Repaint();
            }
        }
    }

    private void HandleAddNode()
    {
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
    }
    private void NodeSelectionMade(NodeType selection)
    {
        _graph.AddNewNode(selection, _newNodeSelectionMenu.Position - _graph.Center);

        _newNodeSelectionMenu = null;
    }
    private void HandleAddElementToNode()
    {
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

            string text = menu.Options[i].ToString();
/*            if (typeof(T) == typeof(Type))
            {
                text = ((Type)(object)menu.Options[i]).Name;
            }*/

            EditorGUI.LabelField(optionRect, text);

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

    #region Node

    private void DrawNode(GraphNode node)
    {
        if (HandleDeleteNode(node)) { return; }

        EditorGUI.indentLevel = 0;

        int cursorHeight = 0;
        float nodeHeight = PROPERTY_HEIGHT * (GetNodeHeight(node) + 2);

        nodeHeight += (node.IsUnfolded ? ELEMENT_PADDING * 0.5f : 0.0f);
        Rect nodeRect = new Rect(node.Position + _graph.Center, new Vector2(PROPERTY_WIDTH, nodeHeight));
        
        Color _outlineColor = GetOutlineColor(node);
        Handles.DrawSolidRectangleWithOutline(nodeRect, BODY_COLOR, _outlineColor);

        if (OnObjectDraggedIn(nodeRect, out GameObject draggedInObject))
        {
            HandleObjectDraggedIn(node, draggedInObject);
        }

        Rect headerRect = ReserveRect(node, nodeRect, ref cursorHeight);
        HandleSelectNode(node, nodeRect);
        HandleNodeDragged(node, headerRect);
        DrawNodeHeader(headerRect, node);

        Rect useBlinkingSettingRect = ReserveRect(node, nodeRect, ref cursorHeight);
        node.SetWaitForBlink(EditorGUI.Toggle(useBlinkingSettingRect, "Use Blinking", node.WaitForBlink));

        if (node.Type != NodeType.Entry)
        {
            Rect connectorInRect = new Rect(nodeRect.position + new Vector2(-CONNECTOR_RECT_SIZE, 0), Vector2.one * CONNECTOR_RECT_SIZE);
            EditorGUI.DrawRect(connectorInRect, new Color(0.2f, 1.0f, 1.0f, 1));
            HandleAddNodeConnection(node, connectorInRect, NodeConnectionType.In);
        }

        Rect connectorOutRect = new Rect(nodeRect.position + new Vector2(PROPERTY_WIDTH, 0), Vector2.one * CONNECTOR_RECT_SIZE);
        EditorGUI.DrawRect(connectorOutRect, new Color(1.0f, 1.0f, 0.2f, 1));
        HandleAddNodeConnection(node, connectorOutRect, NodeConnectionType.Out);

        DrawNodeConnection(node, nodeRect);

        if (!node.IsUnfolded) { return; }

        for (int i = 0; i < node.Elements.Length; i++)
        {
            DrawElement(node, node.Elements[i], nodeRect, ref cursorHeight);
        }
    }

    private Color GetOutlineColor(GraphNode node)
    {
        Color _outlineColor = OUTLINE_COLOR;
        if (_graph.CurrentNode == node)
        {
            _outlineColor = Color.yellow;
        }
        else if (node.Id == _selectedNodeId)
        {
            _outlineColor = SELECTED_COLOR;
        }

        return _outlineColor;
    }

    private void DrawElement(GraphNode node, NodeElement element, Rect nodeRect, ref int cursorHeight)
    {
        EditorGUI.indentLevel = 0;
        SerializedObject serializedObject = new SerializedObject(element);

        Rect elementRect = ReserveRect(node, nodeRect, ref cursorHeight, ELEMENT_PADDING);
        elementRect.height *= GetElementHeight(element);

        Color _outlineColor = OUTLINE_COLOR;
        if (node == _graph.CurrentNode && node.Type == NodeType.Condition)
        {
            if (element.ConditionIsMet())
            {
                _outlineColor = Color.green;
            } 
            else
            {
                _outlineColor = Color.red;
            }
        }

        Handles.DrawSolidRectangleWithOutline(elementRect, BODY_COLOR, _outlineColor);

        Rect foldoutRect = new Rect(elementRect.position, new Vector2(elementRect.width, PROPERTY_HEIGHT));
        string name = $"({element.ReferencedObject.name}) {element.ToString()}";
        element.SetUnfolded(EditorGUI.Foldout(foldoutRect, element.IsUnfolded, new GUIContent(name)));

        if (HandleRemoveElementButton(foldoutRect))
        {
            //remove this element
            node.RemoveElement(element);
        }

        //Get the potetial issues with the way the element has been set up
        List<string> warnings = new List<string>();
        warnings.AddRange(GetAttributeBasedWarnings(element));

        //Draw the warnings
        DrawWarnings(node, nodeRect, ref cursorHeight, warnings);

        if (!element.IsUnfolded) { return; }

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

    #region Draw
    private void DrawNodeConnection(GraphNode node, Rect nodeRect)
    {
        if (_graph.TryGetNodeFromId(node.NextNodeId, out GraphNode foundNode))
        {
            Handles.DrawAAPolyLine(
                4, 
                node.Position + _graph.Center + new Vector2(nodeRect.width, 0) + Vector2.one * 0.5f * CONNECTOR_RECT_SIZE, 
                foundNode.Position + _graph.Center + new Vector2(-1, 1) * 0.5f * CONNECTOR_RECT_SIZE);
        } 
        else
        {
            node.SetNextNodeId(-1);
            HandleUtility.Repaint();
        }
    }
    private void DrawNodeHeader(Rect rect, GraphNode node)
    {
        GUIStyle headerStyle = new GUIStyle();
        headerStyle.alignment = TextAnchor.MiddleCenter;
        headerStyle.normal.textColor = Color.white;

        node.SetUnfolded(EditorGUI.Foldout(rect, node.IsUnfolded ,node.Type.ToString()));
    }
    private void DrawElementRemoveButton(Rect removeButtonRect, bool highlighted)
    {
        GUIStyle removeButtonStyle = new GUIStyle();
        removeButtonStyle.alignment = TextAnchor.MiddleCenter;
        removeButtonStyle.normal.textColor = highlighted ? Color.red : OUTLINE_COLOR;

        EditorGUI.LabelField(removeButtonRect, new GUIContent("✘"), removeButtonStyle);
    }
    private int GetNodeHeight(GraphNode node)
    {
        int heigth = 0;

        if (!node.IsUnfolded) { return heigth; }

        foreach (NodeElement element in node.Elements)
        {
            heigth += GetElementHeight(element);
        }

        return heigth;
    }
    private int GetElementHeight(NodeElement element)
    {
        int height = 1;
        SerializedObject serializedObject = new SerializedObject(element);

        List<string> warnings = GetAttributeBasedWarnings(element);
        height += warnings.Count * 2;

        //skips to the next variable (skips the "script" field)
        if (!element.IsUnfolded) { return height; }

        SerializedProperty iterator = serializedObject.GetIterator();

        iterator.NextVisible(true);
        while (iterator.NextVisible(true))
        {
            height++;
        }

        return height;
    }
    #endregion

    #region Warnings
    private void DrawWarnings(GraphNode node, Rect nodeRect, ref int cursorHeight, List<string> warnings)
    {
        for (int i = 0; i < warnings.Count; i++)
        {
            Rect warningRect = ReserveRect(node, nodeRect, ref cursorHeight, ELEMENT_PADDING * 2, 2);

            GUIStyle warningStyle = new GUIStyle();
            warningStyle.normal.textColor = Color.yellow;
            warningStyle.wordWrap = true;

            EditorGUI.LabelField(warningRect, warnings[i], warningStyle);
        }

        return;
    }
    private List<string> GetAttributeBasedWarnings(NodeElement element)
    {
        List<string> warnings = new List<string>();
        Attribute[] elementAttributes = Attribute.GetCustomAttributes(element.GetType());
        foreach (Attribute attribute in elementAttributes)
        {
            switch (attribute)
            {
                case RequireComponent:
                    RequireComponent requireComponentAttribute = (RequireComponent)attribute;
                    warnings.AddRange(GetHasRequiredComponentsWarnings(element, requireComponentAttribute));
                    break;
            }
        }
        return warnings;
    }
    private List<string> GetHasRequiredComponentsWarnings(NodeElement element, RequireComponent requireComponentAttribute)
    {
        Type[] types = new Type[] { requireComponentAttribute.m_Type0, requireComponentAttribute.m_Type1, requireComponentAttribute.m_Type2 };
        List<string> warnings = new List<string>();

        foreach (Type type in types)
        {
            if (HasRequiredComponentWarning(element, type, out string warning))
            {
                warnings.Add(warning);
            }
        }
        return warnings;
    }
    private bool HasRequiredComponentWarning(NodeElement element, Type requiredComponentType, out string warning)
    {
        warning = "";
        if (requiredComponentType == null) { return false; }
        if (element.ReferencedObject.GetComponent(requiredComponentType) == null)
        {
            warning = $"Selcted Object({element.ReferencedObject.name}) does not have {requiredComponentType.ToString()} attached!";
            return true;
        }
        return false;
    }
    #endregion

    #region Node Handlers
    private void HandleSelectNode(GraphNode node, Rect rect)
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains(Event.current.mousePosition))
        {
            _nodeSelectedThisFrame = true;
            _selectedNodeId = node.Id;
        }
    }
    private void HandleNodeDragged(GraphNode node, Rect rect)
    {
        //if not left mouse button return
        if (Event.current.button != 0) { return; }
        //if selected node is not this node return
        if (node.Id != _selectedNodeId) { return; }

        //if mouse button just pressed start dragging node
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains(Event.current.mousePosition))
        {
            _isDraggingNode = true;
            _mousePosition = Event.current.mousePosition;
        }

        //calculate chnage in position
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 && _isDraggingNode)
        {
            HandleUtility.Repaint();
            EditorUtility.SetDirty(_graph);
            node.ChangePosition(GetMouseDragChange());
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
    private void HandleObjectDraggedIn(GraphNode node, GameObject draggedInObject)
    {
        Type[] allNodeElementsTypes = _allnodeElementTypes;
        List<Type> selectedNodeElementTypes = new List<Type>();
        for (int i = 0; i < allNodeElementsTypes.Length; i++)
        {
            if (allNodeElementsTypes[i].IsAbstract) { continue; }

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
        _newElementSelectionMenu = new SelectionMenu<Type>(Event.current.mousePosition, selectedNodeElementTypes.ToArray());
        HandleUtility.Repaint();
    }
    private bool HandleDeleteNode(GraphNode node)
    {
        if ((Event.current.keyCode == KeyCode.Delete || Event.current.keyCode == KeyCode.Backspace) && _selectedNodeId == node.Id)
        {
            _graph.RemoveNodeById(node.Id);
            HandleUtility.Repaint();
            return true;
        }
        return false;
    }
    private bool HandleRemoveElementButton(Rect lineRect)
    {
        Rect removeButtonRect = new Rect(lineRect);
        removeButtonRect.x += removeButtonRect.width - PROPERTY_HEIGHT;
        removeButtonRect.width = PROPERTY_HEIGHT;

        DrawElementRemoveButton(removeButtonRect, removeButtonRect.Contains(Event.current.mousePosition));

        if (removeButtonRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            return true;
        }

        return false;
    }
    #endregion
    
    #region Helper
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
    private bool OnObjectDraggedIn(Rect rect, out GameObject obj)
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
    #endregion

    #endregion

    #region Helper
    private Vector2 GetMouseDragChange()
    {
        Vector2 change = Event.current.mousePosition - _mousePosition;
        _mousePosition = Event.current.mousePosition;
        return change;
    }
    private Rect ReserveRect(GraphNode node, Rect nodeRect, ref int cursorHeight, int horizontalPadding = 0, int height = 1)
    {
        float indent = EditorGUI.indentLevel * 5;
        Rect returnRect = new Rect
            (
            new Vector2(indent, PROPERTY_HEIGHT * cursorHeight) + node.Position + new Vector2(horizontalPadding * 0.5f, 0) + _graph.Center,
            new Vector2(nodeRect.width - indent, height * PROPERTY_HEIGHT) - new Vector2(horizontalPadding, 0)
            );

        cursorHeight += height;
        return returnRect;
    }
    public static Type[] GetAllSubclassesOf(Type baseType)
    {
        return Assembly.GetAssembly(baseType).GetTypes().Where(type => type.IsSubclassOf(baseType)).ToArray();
    }
    #endregion

    #region Enums
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
    #endregion
}
