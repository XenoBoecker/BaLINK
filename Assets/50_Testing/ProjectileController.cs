using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float _lifetime = 10f;
    [SerializeField] private Rigidbody _rb;

    private void Awake()
    {
        gameObject.AddComponent<SelfDestruct>().Initialize(_lifetime);
    }

    private void Start()
    {
        _rb.isKinematic = true;
    }

    internal void Shoot(float shootForce)
    {
        transform.SetParent(null);
        _rb.isKinematic = false;
        _rb.AddForce(transform.forward * shootForce);
    }
}
