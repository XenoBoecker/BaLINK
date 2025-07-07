using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrossbowItem : EquippedItem
{
    [SerializeField] private bool _clickToReload;

    [SerializeField] private float _shootForce;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _arrowSpawnPos;

    [SerializeField] private float arrowSpawnDuration = 3f;

    private bool _isReloading;
    private bool _readyToFire = false;
    private GameObject _spawnedArrow;

    private void Update()
    {
        if (!_isEquipped || _readyToFire || _isReloading || _clickToReload)
        {
            return;
        }

        Reload();
    }

    public override void UseItem()
    {
        base.UseItem();

        if(!_isEquipped)
        {
            return;
        }

        if (_readyToFire)
        {
            Release();
            return;
        }
        else if (_clickToReload && !_isReloading)
        {
            Reload();
            return;
        }
    }

    private void Reload()
    {
        Debug.Log("Reloading Crossbow");
        _animator.SetTrigger("Reload");
        StartCoroutine(ReloadRoutine(_animator.GetCurrentAnimatorStateInfo(0).length));
    }

    private IEnumerator ReloadRoutine(float delay)
    {
        _isReloading = true;
        if (_spawnedArrow != null) { _readyToFire = true; }

        arrowSpawnDuration = delay * 0.5f;

        //yield return new WaitForSeconds(delay * 0.45f - arrowSpawnDuration);

        
        _spawnedArrow = Instantiate(_arrowPrefab, _arrowSpawnPos);
        _spawnedArrow.transform.localPosition = Vector3.zero;
        _spawnedArrow.transform.localRotation = Quaternion.identity;

        Material arrowMat = _spawnedArrow.GetComponentInChildren<MeshRenderer>().material;
        
        for (float i = 0; i < arrowSpawnDuration; i += Time.deltaTime)
        {
            arrowMat.SetFloat("_Cutoff_Height", (i / arrowSpawnDuration));
            yield return null;
        }

        _readyToFire = true;
        _isReloading = false;
    }

    private void Release()
    {
        Debug.Log("Releasing Crossbow Arrow");
        if (!_isEquipped)
        {
            return;
        }

        _animator.SetTrigger("Fire");
        if (!_readyToFire)
        {
            StopAllCoroutines();
            Destroy(_spawnedArrow);
            return;
        }

        _spawnedArrow.GetComponent<ProjectileController>().Shoot(_shootForce);

        _spawnedArrow = null;
        _readyToFire = false;
    }

    private void Released(InputAction.CallbackContext context)
    {
        if (!_isEquipped)
        {
            return;
        }

        _animator.SetTrigger("Fire");
        if (!_readyToFire) 
        {
            StopAllCoroutines();
            Destroy(_spawnedArrow);
            return; 
        }

        _spawnedArrow.GetComponent<ProjectileController>().Shoot(_shootForce);

        _spawnedArrow = null;
        _readyToFire = false;
    }
}
