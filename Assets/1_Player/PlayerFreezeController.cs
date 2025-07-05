using ECM.Components;
using ECM.Controllers;
using UnityEngine;

public class PlayerFreezeController : MonoBehaviour
{
    private CharacterMovement _characterMovement;
    private BaseFirstPersonController _baseFirstPersonController;

    private bool _isFrozen;
    public bool IsFrozen => _isFrozen;

    void Awake()
    {
        _characterMovement = FindAnyObjectByType<CharacterMovement>();
        _baseFirstPersonController = _characterMovement.GetComponent<BaseFirstPersonController>();
    }

    public void SetFreeze(bool isFrozen)
    {
        _isFrozen = isFrozen;
        _characterMovement.Pause(isFrozen);
        _characterMovement.enabled = !isFrozen; // Disable player movement
        _baseFirstPersonController.enabled = !isFrozen; // Disable the base first person controller
    }
}
