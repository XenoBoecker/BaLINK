using System;
using Unity.VisualScripting;
using UnityEngine;

public class Hitable : MonoBehaviour
{
    [SerializeField] private bool shootThroughObject;
    public bool ShootThroughObject => shootThroughObject; // Expose the property to other scripts
    [SerializeField] private GameObject objectBeforeExplosion;
    [SerializeField] private Rigidbody[] destructionParts;
    [SerializeField] private float destructionForce = 10f; // Force applied to destruction parts when hit
    [SerializeField] private int hitPoints = 1; // Number of hits the object can take before being destroyed
    int currentHitPoints;

    Rigidbody rb;

    public event Action OnHit; // Event to notify when the object is hit

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

    public void Hit(Transform projectile)
    {
        currentHitPoints--; // Increment the hit points when hit by a crossbow bolt
        // Logic for when the object is hit by a crossbow bolt
        Debug.Log($"{gameObject.name} has been hit!");

        OnHit?.Invoke(); // Invoke the OnHit event to notify subscribers

        if (currentHitPoints <= 0)
        {
            DestroyObject(projectile);
        }
    }

    private void DestroyObject(Transform projectile)
    {
        if(destructionParts != null && destructionParts.Length > 0)
        {
            objectBeforeExplosion.SetActive(false); // Deactivate the original object before explosion
            GetComponent<Collider>().enabled = false; // Disable the collider to prevent further interactions
            for (int i = 0; i < destructionParts.Length; i++)
            {
                destructionParts[i].gameObject.SetActive(true); // Activate the destruction parts
                destructionParts[i].AddExplosionForce(destructionForce, transform.position, 5f); // Apply explosion force to each part
            }
        }
        else
        {
            if (rb != null)
            {
                rb.isKinematic = false; // Make the Rigidbody non-kinematic to allow physics interactions
                rb.AddExplosionForce(destructionForce, projectile.position, 5f); // Apply explosion force to the object
            }
        }

        Debug.Log($"{gameObject.name} has been destroyed!");
    }
}
