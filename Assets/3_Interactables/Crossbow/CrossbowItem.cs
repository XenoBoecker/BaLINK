using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrossbowItem : EquippedItem
{
    [SerializeField] private bool _clickToReload;

    [SerializeField] private float _shootForce;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _arrowSpawnPos;


    [SerializeField] private float _shootInputBufferTime = 0.3f;
    float _shootInputBufferTimer = 0f;

    private float arrowSpawnDuration = 3f;

    private bool _isReloading;
    private bool _isReleasing;
    private bool _readyToFire = false;
    private GameObject _spawnedArrow;

    private void Update()
    {
        _shootInputBufferTimer -= Time.deltaTime;

        if (!_isEquipped || _isReloading || _clickToReload)
        {
            return;
        }

        if (_readyToFire)
        {
            if(_shootInputBufferTimer > 0f)
            {
                Release();
            }
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
        else
        {
            _shootInputBufferTimer = _shootInputBufferTime;
        }
    }

    private void Reload()
    {
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
        if (!_isEquipped)
        {
            return;
        }

        if (!_readyToFire)
        {
            StopAllCoroutines();
            Destroy(_spawnedArrow);
            return;
        }

        if(_isReleasing)
        {
            return; // Already releasing, ignore further input
        }

        _shootInputBufferTimer = 0f;

        _animator.SetTrigger("Fire");

        StartCoroutine(ReleaseRoutine(_animator.GetCurrentAnimatorStateInfo(0).length));
    }

    IEnumerator ReleaseRoutine(float delay)
    {
        _isReleasing = true;

        _spawnedArrow.GetComponent<ProjectileController>().Shoot(_shootForce);
        yield return new WaitForSeconds(delay * 0.5f);

        _spawnedArrow = null;
        _isReleasing = false;
        _readyToFire = false;
    }
}
