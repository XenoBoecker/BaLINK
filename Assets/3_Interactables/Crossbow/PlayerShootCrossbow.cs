using UnityEngine;

public class PlayerShootCrossbow : EquippedItem
{
    [SerializeField] private float shootRange = 10f; // Range within which the player can shoot
    [SerializeField] private ArrowDestroyObjectOnCollision crossbowBoltPrefab; // Prefab for the crossbow bolt
    [SerializeField] private Transform shootPoint; // Point from which the crossbow bolt is shot
    [SerializeField] private float boltSpeed = 20f; // Speed of the crossbow bolt
    [SerializeField] private float boltLifetime = 1f; // Lifetime of the crossbow bolt before it is destroyed

    public override void UseItem() 
    {
        Shoot();
    }

    private void Shoot()
    {
        // Instantiate the crossbow bolt at the shoot point
        ArrowDestroyObjectOnCollision bolt = Instantiate(crossbowBoltPrefab, shootPoint.position, shootPoint.rotation);

        // Get the Rigidbody component of the bolt and set its velocity
        Rigidbody rb = bolt.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = shootPoint.forward * boltSpeed;
        }
        // Optionally, you can add logic to destroy the bolt after a certain time or when it hits something
        Destroy(bolt, boltLifetime); // Destroy the bolt after 5 seconds
    }

}
