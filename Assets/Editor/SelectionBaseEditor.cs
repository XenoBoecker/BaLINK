using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SelectionBase))]
public class SelectionBaseEditor : Editor
{
    private void OnSceneGUI()
    {
        SelectionBase selectionBase = target as SelectionBase;

        for (int i = 0; i < selectionBase.transform.childCount; i++)
        {
            if (Selection.transforms.Contains(selectionBase.transform.GetChild(i)))
            {
                Selection.activeTransform = selectionBase.transform;
                return;
            }
        }
    }
}
