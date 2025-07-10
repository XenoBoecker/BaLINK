using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float _lifetime = 10f;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private TrailRenderer _trailRenderer;

    [SerializeField] private bool _doCustomContinousCollisionDetection = true;
    private Vector3 _previousPosition;
    private bool _hasBeenShot;

    private void Start()
    {
        _rb.isKinematic = true;

        _hasBeenShot = false;

        if (_trailRenderer != null)
        {
            _trailRenderer.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (!_hasBeenShot)
        {
            return;
        }

        OnHit(collider);
    }

    private void LateUpdate()
    {
        if (!_doCustomContinousCollisionDetection || !_hasBeenShot)
        {
            return;
        }
        
        Vector3 lastUpdatedMovement = transform.position - _previousPosition;

        bool hit = Physics.Raycast(_previousPosition, lastUpdatedMovement, out RaycastHit hitInfo, 20*lastUpdatedMovement.magnitude);

        if (hit)
        {
            OnHit(hitInfo.collider);
        }
        
        _previousPosition = transform.position;
    }

    void OnHit(Collider collider)
    {
        Hitable hitable = collider.gameObject.GetComponent<Hitable>();

        if (hitable == null)
        {
            return;
        }

        hitable.Hit(transform);

        if (hitable.ShootThroughObject)
        {
            return;
        }

        transform.parent = collider.transform;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Make the Rigidbody kinematic to stop further movement
            rb.linearVelocity = Vector3.zero; // Stop the bolt's movement
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false; // Disable gravity to prevent falling
        }
    }

    internal void Shoot(float shootForce)
    {
        _hasBeenShot = true;

        transform.SetParent(null);
        _rb.isKinematic = false;
        _rb.AddForce(transform.forward * shootForce);

        _previousPosition = transform.position;

        gameObject.AddComponent<SelfDestruct>().Initialize(_lifetime);

        if (_trailRenderer != null)
        {
            _trailRenderer.enabled = true;
        }
    }
}
