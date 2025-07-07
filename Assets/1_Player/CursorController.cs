using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    private PlayerInteractor _playerInteractor;
    [SerializeField] private Image _cursorImageComponent;

    private void Awake()
    {
        _playerInteractor = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerInteractor>();
    }

    private void Update()
    {
        Debug.Log(_playerInteractor.IsMouseHoverOverInteractable());
        _cursorImageComponent.enabled = _playerInteractor.IsMouseHoverOverInteractable();
    }
}
