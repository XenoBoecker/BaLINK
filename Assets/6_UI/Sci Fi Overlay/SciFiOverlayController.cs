using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(EquippedItem))]
public class SciFiOverlayController : MonoBehaviour
{
    [SerializeField] Material _effectMaterial;
    [SerializeField] string _effectKeyword = "_IsActive";
    [SerializeField] bool _enableOnUpdate = false;

    private EquippedItem _equippedItem;

    private void Awake()
    {
        _equippedItem = GetComponent<EquippedItem>();
        if (_equippedItem == null)
        {
            Debug.LogError("EquippedItem component is missing on the GameObject.", this);
            return;
        }
        DeactivateEffect();
    }

    private void OnEnable()
    {
        if (_equippedItem != null)
        {
            _equippedItem.OnEquippedChanged += HandleEquippedChanged;
            if (_equippedItem.IsEquipped)
            {
                ActivateEffect();
            }
        }
    }

    private void Update()
    {
        if (_enableOnUpdate)
        {
            _effectMaterial.SetFloat(_effectKeyword, 1.0f);
        }
        else
        {
            _effectMaterial.SetFloat(_effectKeyword, 0.0f);
        }
    }

    private void OnDisable()
    {
        if (_equippedItem != null)
        {
            _equippedItem.OnEquippedChanged -= HandleEquippedChanged;
            DeactivateEffect();
        }
    }

    private void HandleEquippedChanged(bool isEquipped)
    {
        if (isEquipped)
        {
            ActivateEffect();
        }
        else
        {
            DeactivateEffect();
        }
    }

    public void ActivateEffect()
    {
        Debug.Log("Activating Full Screen Effect");
        _effectMaterial.SetFloat(_effectKeyword, 1.0f);
    }

    public void DeactivateEffect()
    {
        Debug.Log("Deactivating Full Screen Effect");
        _effectMaterial.SetFloat(_effectKeyword, 0.0f);
    }
}
