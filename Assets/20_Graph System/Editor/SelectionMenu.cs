using UnityEditor;
using UnityEngine;

public class SelectionMenu<T>
{
    private Vector2 _position;
    private Vector2 _singleSelectionSize;
    private T[] _options;
    
    public Vector2 Position => _position;
    public Vector2 SingleSelectionSize => _singleSelectionSize;
    public T[] Options => _options;

    public SelectionMenu(Vector2 position, T[] options)
    {
        _position = position;
        _singleSelectionSize = new Vector2(200, EditorGUIUtility.singleLineHeight);
        _options = options;
    }
}
