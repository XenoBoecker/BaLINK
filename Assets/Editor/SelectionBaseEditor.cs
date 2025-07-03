using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SelectionBase))]
public class SelectionBaseEditor : Editor
{
    private void OnEnable()
    {
        SelectionBase selectionBase = target as SelectionBase;
        PrefabUtility.RevertPrefabInstance(selectionBase.gameObject, InteractionMode.AutomatedAction);
    }
}
