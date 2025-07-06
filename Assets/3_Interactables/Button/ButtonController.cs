using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : Interactable
{
    [SerializeField] private float _clickCooldown = 0;
    [SerializeField] private UnityEvent _onClick;

    bool _isOnCooldown;

    protected override void Interact()
    {
        if (_isOnCooldown) { return; }

        _onClick?.Invoke();
        InteractedSuccessfully();
        StartCoroutine(DoCooldown());
    }

    private IEnumerator DoCooldown()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(_clickCooldown);
        _isOnCooldown = false;
    }
}
