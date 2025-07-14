using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode] 
public class LineRendererPointController : MonoBehaviour
{
    [SerializeField] private Transform[] _linePointTargets;
    private LineRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        if (_renderer == null) { return; }

        for (int i = 0; i < _linePointTargets.Length; i++)
        {
            if (_renderer.positionCount <= i) { continue; }

            _renderer.SetPosition(i, _linePointTargets[i].transform.position);
        }
    }
}
