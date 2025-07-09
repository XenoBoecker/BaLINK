using UnityEditor;
using UnityEngine;

public static class EditorGizmos
{
    [DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.Pickable | GizmoType.Selected)]
    public static void CurveGizmos(WireGenerator generator, GizmoType type)
    {
        bool selected = (type & GizmoType.Selected) > 0;
        Gizmos.matrix = generator.gameObject.transform.localToWorldMatrix;
        Gizmos.color = new Color(0, 0, 0, 0);
        Gizmos.DrawMesh(generator.GetMesh());
    }
}

