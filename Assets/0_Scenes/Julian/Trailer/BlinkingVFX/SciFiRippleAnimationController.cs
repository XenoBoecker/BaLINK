using UnityEngine;

[ExecuteInEditMode]
public class SciFiAnimationController : MonoBehaviour
{
    [SerializeField] bool _active;
    [SerializeField] float _radius;
    [SerializeField] float _transparency;
    [SerializeField] Material _material;

    bool _toggle;

    // Update is called once per frame
    void Update()
    {
        if (_active)
        {
            _toggle = true;
            _material.SetFloat("_ShockwaveRadius", _radius);
            _material.SetFloat("_Transparency", _transparency);
            return;
        }

        if (_toggle)
        {
            _toggle = false;
            _material.SetFloat("_ShockwaveRadius", _radius);
            _material.SetFloat("_Transparency", _transparency);
        }
    }

    private void OnDisable()
    {
        _material.SetFloat("_ShockwaveRadius", 99999f);
        _material.SetFloat("_Transparency", 0f);
    }
}
