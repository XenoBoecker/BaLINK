using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CrossbowItem : EquippedItem
{
    [SerializeField] private bool _clickToReload;

    [SerializeField] private float _shootForce;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _arrowSpawnPos;


    [SerializeField] private float _inputBufferTime = 0.3f;
    float _inputBufferTimer = 0f;

    [SerializeField] private UnityEvent _onReload;
    [SerializeField] private UnityEvent _onRelease;

    private float arrowSpawnDuration = 3f;

    private bool _isReloading;
    private bool _isReleasing;
    private bool _readyToFire = false;
    private GameObject _spawnedArrow;

    private void Update()
    {
        _inputBufferTimer -= Time.deltaTime;

        if (!_isEquipped || _isReloading)
        {
            return;
        }

        if (_readyToFire)
        {
            if(_inputBufferTimer > 0f)
            {
                Release();
            }
            return;
        }

        if (_clickToReload)
        {
            if (!_isReloading && _inputBufferTimer > 0f)
            {
                Reload();
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

        if (_readyToFire && !_isReleasing)
        {
            Release();
            return;
        }
        else if (_clickToReload && !_isReloading && !_isReleasing)
        {
            Debug.Log("Reloading on click");
            Reload();
            return;
        }
        else
        {
            Debug.Log("Buffer");
            _inputBufferTimer = _inputBufferTime;
        }
    }

    private void Reload()
    {
        _inputBufferTimer = 0f;

        _onReload.Invoke();

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

        _inputBufferTimer = 0f;

        _onRelease.Invoke();
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
