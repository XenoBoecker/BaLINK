using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshFilter))]
public class MeshFilterEditor : Editor
{
    private Mesh displayedMesh;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MeshFilter filter = target as MeshFilter;
        displayedMesh = filter.sharedMesh;

        if (filter.TryGetComponent(out MeshCollider collider))
        {
            collider.sharedMesh = filter.sharedMesh;
        }
    }
}
