using UnityEngine;

public class EquippedItemUI : MonoBehaviour
{
    [SerializeField] private EquippedItem _equippedItem;
    [SerializeField] private GameObject _ui;

    private void Start()
    {
        if(_equippedItem == null)
        {
            Debug.LogError("EquippedItem is not assigned in EquippedItemUI.");
            return;
        }

        if(_ui == null)
        {
            Debug.LogError("UI GameObject is not assigned in EquippedItemUI.");
            return;
        }

        UpdateUI(_equippedItem != null && _equippedItem.IsEquipped);
    }
    private void OnEnable()
    {
        if (_equippedItem != null)
        {
            _equippedItem.OnEquippedChanged += UpdateUI;
            UpdateUI(_equippedItem.HasBeenUsedThisGame);
        }
    }
    private void OnDisable()
    {
        if (_equippedItem != null)
        {
            _equippedItem.OnEquippedChanged -= UpdateUI;
        }
    }
    private void UpdateUI(bool isEquipped)
    {
        _ui.SetActive(isEquipped);
    }
}
