using UnityEngine;

public class SwapMaterialController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _targetRenderer;
    [SerializeField] private Material _targetMaterial;

    private Material _originalMaterial;

    private void Awake()
    {
        _originalMaterial = _targetRenderer.material;
    }

    public void SwapMaterial()
    {
        _targetRenderer.material = _targetMaterial;
        _targetMaterial = _originalMaterial;
        _originalMaterial = _targetRenderer.material;
    }
}
