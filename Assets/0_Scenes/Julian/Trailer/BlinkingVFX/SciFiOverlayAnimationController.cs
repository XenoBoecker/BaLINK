using UnityEngine;

[ExecuteInEditMode]
public class SciFiOverlayAnimationController : MonoBehaviour
{
    [SerializeField] bool _active;
    [SerializeField] float _transparency;
    [SerializeField] Material _material;

    bool _toggle;

    // Update is called once per frame
    void Update()
    {
        if (_active)
        {
            _toggle = true;
            _material.SetFloat("_IsActive", 1f);
            _material.SetFloat("_Transparency", _transparency);
            return;
        }

        if (_toggle)
        {
            _toggle = false;
            _material.SetFloat("_IsActive", 1f);
            _material.SetFloat("_Transparency", _transparency);
        }
    }

    private void OnDisable()
    {
        _material.SetFloat("_IsActive", 0f);
        _material.SetFloat("_Transparency", 0f);
    }
}
