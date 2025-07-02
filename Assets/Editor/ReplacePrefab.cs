using UnityEditor;
using UnityEngine;

public class ReplacePrefab
{
    static private int _controlId = -1;

    [InitializeOnLoadMethod]
    static void Init()
    {
        EditorApplication.update += Update;
        EditorWindow.windowFocusChanged += WindowFocusChanged;
    }

    [MenuItem("CONTEXT/Transform/Replace Multiple...", true)]
    static bool ValidateCreateEmptyParentAtOrigin()
    {
        if (Selection.transforms != null && Selection.transforms.Length > 1)
        {
            return true;
        }
        return false;
    }

    [MenuItem("CONTEXT/Transform/Replace Multiple...", false)]
    static void CreateEmptyParentAtOrigin(MenuCommand command)
    {
        _controlId = EditorGUIUtility.GetControlID(FocusType.Passive) + 100;
        EditorGUIUtility.ShowObjectPicker<GameObject>(command.context as GameObject, false, "", _controlId);
        _selectedTransforms = Selection.transforms;
    }

    static UnityEngine.Object _prevSelectedObject = null;
    static Transform[] _selectedTransforms;

    static private void Update()
    {
        if (_controlId == -1) { return; }

        if (EditorGUIUtility.GetObjectPickerControlID() == _controlId/* Event.current.commandName == "ObjectSelectorUpdated"*/)
        {
            if (EditorGUIUtility.GetObjectPickerObject() != _prevSelectedObject && _prevSelectedObject != null)
            {
                SwapObjects(EditorGUIUtility.GetObjectPickerObject());
            }
            _prevSelectedObject = EditorGUIUtility.GetObjectPickerObject();
        }
    }

    private static void SwapObjects(UnityEngine.Object selectedObject)
    {
        if (selectedObject as GameObject == null) { return; }

        for (int i = 0; i < _selectedTransforms.Length; i++)
        {
            if (!PrefabUtility.IsAnyPrefabInstanceRoot(_selectedTransforms[i].gameObject)) { continue; }

            Vector3 tempPosition = _selectedTransforms[i].position;
            Quaternion tempRotation = _selectedTransforms[i].rotation;
            Vector3 tempScale = _selectedTransforms[i].localScale;
            Transform parent = _selectedTransforms[i].parent;

            GameObject newObject = PrefabUtility.InstantiatePrefab(selectedObject) as GameObject;
            if (newObject == null) { return; }

            if (parent != null) { newObject.transform.SetParent(parent); }
            newObject.transform.position = tempPosition;
            newObject.transform.rotation = tempRotation;
            newObject.transform.localScale = tempScale;

            GameObject.DestroyImmediate(_selectedTransforms[i].gameObject);
            _selectedTransforms[i] = newObject.transform;
        }
    }

    private static void WindowFocusChanged()
    {
        if (_prevSelectedObject == null)
        {
            return;
        }
    }
}
