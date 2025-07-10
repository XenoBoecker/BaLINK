using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Hitable : MonoBehaviour
{
    [SerializeField] private bool shootThroughObject;
    public bool ShootThroughObject => shootThroughObject; // Expose the property to other scripts
    [SerializeField] private bool hideOnDestruction = false; // Option to hide this object on destruction. Used when playing VFX via UnityEvents instead.
    [SerializeField] private GameObject objectBeforeExplosion;
    [SerializeField] private Rigidbody[] destructionParts;
    [SerializeField] private float destructionForce = 200f; // Force applied to destruction parts when hit
    [SerializeField] private int hitPoints = 1; // Number of hits the object can take before being destroyed
    int currentHitPoints;

    Rigidbody rb;

    public event Action OnHit; // Event to notify when the object is hit
    [SerializeField] private UnityEvent _onHit;

    public event Action<Hitable> OnDestroyed;
    [SerializeField] private UnityEvent _onDestroyed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if(rb != null) rb.isKinematic = true; // Make the Rigidbody kinematic initially to prevent physics interactions

        if (destructionParts != null)
        {
            for (int i = 0; i < destructionParts.Length; i++)
            {
                destructionParts[i].gameObject.SetActive(false);
            }
        }
    }

    public virtual void Hit(Transform projectile)
    {
        currentHitPoints--; // Increment the hit points when hit by a crossbow bolt
        // Logic for when the object is hit by a crossbow bolt
        Debug.Log($"{gameObject.name} has been hit!");

        OnHit?.Invoke(); // Invoke the OnHit event to notify subscribers
        _onHit.Invoke();

        if (currentHitPoints <= 0)
        {
            DestroyObject(projectile);
        }
    }

    private void DestroyObject(Transform projectile)
    {
        if(!hideOnDestruction && destructionParts != null && destructionParts.Length > 0)
        {
            objectBeforeExplosion.SetActive(false); // Deactivate the original object before explosion
            GetComponent<Collider>().enabled = false; // Disable the collider to prevent further interactions

            for (int i = 0; i < destructionParts.Length; i++)
            {
                destructionParts[i].gameObject.SetActive(true); // Activate the destruction parts
                                                                //destructionParts[i].AddExplosionForce(destructionForce, transform.position, 5f); // Apply explosion force to each part
                Vector3 forceDir = (transform.position - FindAnyObjectByType<PlayerInteractor>().transform.position).normalized;
                destructionParts[i].AddForce(destructionForce * forceDir, ForceMode.Impulse);
            }
        }
        else if (!hideOnDestruction)
        {
            if (rb != null)
            {
                rb.isKinematic = false; // Make the Rigidbody non-kinematic to allow physics interactions
                //rb.AddExplosionForce(destructionForce, projectile.position, 5f); // Apply explosion force to the object
                Vector3 forceDir = (transform.position - FindAnyObjectByType<PlayerInteractor>().transform.position).normalized;
                rb.AddForce(destructionForce * forceDir, ForceMode.Impulse);
            }
        }
        else
        {
            objectBeforeExplosion.SetActive(false); // Deactivate the original object before explosion
            GetComponent<Collider>().enabled = false; // Disable the collider to prevent further interactions
        }

        Debug.Log($"{gameObject.name} has been destroyed!");

        OnDestroyed?.Invoke(this);
        _onDestroyed.Invoke();
    }
}
