using UnityEngine;

[ExecuteInEditMode]
public class SciFiOverlayController : MonoBehaviour
{
    [SerializeField] Material _effectMaterial;
    [SerializeField] string _effectKeyword = "IsActive";

    bool _isActive = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleEffect();
        }
    }

    void ToggleEffect()
    {
        _isActive = !_isActive;
        if (_isActive)
        {
            ActivateEffect();
        }
        else
        {
            DeactivateEffect();
        }
    }



    void ActivateEffect()
    {
        Debug.Log("Activating Full Screen Effect");
        _effectMaterial.SetFloat(_effectKeyword, 1.0f);
    }

    void DeactivateEffect()
    {
        Debug.Log("Deactivating Full Screen Effect");
        _effectMaterial.SetFloat(_effectKeyword, 0.0f);
    }
}
