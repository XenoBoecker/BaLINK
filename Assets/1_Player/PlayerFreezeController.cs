using ECM.Components;
using ECM.Controllers;
using UnityEngine;

public class PlayerFreezeController : MonoBehaviour
{
    private CharacterMovement _characterMovement;
    private BaseFirstPersonController _baseFirstPersonController;

    private int _freezeCounter;
    public bool IsFrozen => _freezeCounter > 0;

    void Awake()
    {
        _characterMovement = FindAnyObjectByType<CharacterMovement>();
        _baseFirstPersonController = _characterMovement.GetComponent<BaseFirstPersonController>();
    }

    public void SetFreeze(bool isFrozen)
    {
        bool wasFrozen = IsFrozen;
        if (isFrozen)
        {
            _freezeCounter++;
        }
        else
        {
            _freezeCounter--;
        }

        if(wasFrozen == IsFrozen)
        {
            return;
        }

        _characterMovement.Pause(IsFrozen);
        _characterMovement.enabled = !IsFrozen; // Disable player movement
        _baseFirstPersonController.enabled = !IsFrozen; // Disable the base first person controller
    }
}
