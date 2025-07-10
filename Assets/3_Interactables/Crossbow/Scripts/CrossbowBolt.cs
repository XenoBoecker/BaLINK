using System;
using UnityEngine;

public class CrossbowBolt : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        Hitable hitable = collider.gameObject.GetComponent<Hitable>();

        if(hitable == null)
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
}
