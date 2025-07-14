using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class DepthOfFieldAnimationController : MonoBehaviour
{
    [SerializeField] bool _active;
    [SerializeField] bool _overwriteFocusDistance = false;
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
            Debug.Log("DOF is running " + GetFocusDistance());
            return;
        }

        if (_toggle)
        {
            _toggle = false;
            _volume.profile.TryGet(out _dof);
            _dof.focusDistance.value = GetFocusDistance();
        }
    }

    private void FixedUpdate()
    {
        Update();
    }

    private void OnDisable()
    {
        _volume.profile.TryGet(out _dof);
        _dof.focusDistance.value = GetFocusDistance();
    }

    private float GetFocusDistance()
    {
        if (_focusObject == null || _overwriteFocusDistance)
        {
            return _focusDistance;
        }
        else
        {
            return Vector3.Distance(transform.position, _focusObject.transform.position); //new Vector3(0, 1.452f, 52.229f));
        }
    }
}
