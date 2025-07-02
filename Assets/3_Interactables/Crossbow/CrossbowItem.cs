using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrossbowItem : EquippedItem
{
    [SerializeField] private float _shootForce;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _arrowSpawnPos;

    private InputSystem_Actions _input;
    private bool _readyToFire = false;
    private GameObject _spawnedArrow;

    private void Awake()
    {
        _input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Interact.canceled += Released;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Interact.canceled -= Released;
    }

    public override void UseItem()
    {
        base.UseItem();

        //TODO - draw crossbow
        //play anim
        //spawn arrow
        _animator.SetTrigger("Reload");
        StartCoroutine(Reload(_animator.GetCurrentAnimatorStateInfo(0).length));
    }

    private IEnumerator Reload(float delay)
    {
        if (_spawnedArrow != null) { _readyToFire = true; }

        yield return new WaitForSeconds(delay * 0.45f);

        _readyToFire = true;
        
        _spawnedArrow = Instantiate(_arrowPrefab, _arrowSpawnPos);
        _spawnedArrow.transform.localPosition = Vector3.zero;
        _spawnedArrow.transform.localRotation = Quaternion.identity;

        Material arrowMat = _spawnedArrow.GetComponentInChildren<MeshRenderer>().material;
        
        float arrowSpawnDuration = 0.3f;
        for (float i = 0; i < arrowSpawnDuration; i += Time.deltaTime)
        {
            arrowMat.SetFloat("_Cutoff_Height", 10 * (i / arrowSpawnDuration));
            yield return null;
        }
    }

    private void Released(InputAction.CallbackContext context)
    {
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
