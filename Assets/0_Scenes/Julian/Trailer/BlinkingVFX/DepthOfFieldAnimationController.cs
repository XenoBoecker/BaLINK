using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class DepthOfFieldAnimationController : MonoBehaviour
{
    [SerializeField] bool _active;
    [SerializeField] float _focusDistance = 1;
    [SerializeField] Transform _focusObject;
    [SerializeField] Volume _volume;

    DepthOfField _dof;

    bool _toggle;


    // Update is called once per frame
    void Update()
    {       
        if (_active)
        {
            _toggle = true;
            _volume.profile.TryGet(out _dof);
            _dof.focusDistance.value = GetFocusDistance();
            return;
        }

        if (_toggle)
        {
            _toggle = false;
            _volume.profile.TryGet(out _dof);
            _dof.focusDistance.value = GetFocusDistance();
        }
    }

    private void OnDisable()
    {
        _volume.profile.TryGet(out _dof);
        _dof.focusDistance.value = GetFocusDistance();
    }

    private float GetFocusDistance()
    {
        if (_focusObject == null)
        {
            return _focusDistance;
        }
        else
        {
            return Vector3.Distance(transform.position, _focusObject.transform.position);
        }
    }
}
