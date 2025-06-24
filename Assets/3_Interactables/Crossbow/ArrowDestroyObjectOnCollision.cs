using System;
using UnityEngine;

public class ArrowDestroyObjectOnCollision : MonoBehaviour
{
    [SerializeField] LayerMask nonDestructible;
    [SerializeField] private bool destroyThisOnCollision;
    private void OnCollisionEnter(Collision collision)
    {
        if (destroyThisOnCollision)
        {
            Destroy(gameObject, 0.01f);
        }

        if (collision.gameObject.layer == nonDestructible)
        {
            // If the collided object is non-destructible, ignore the collision
            return;
        }

        print(collision.gameObject.name);


        Destroy(collision.gameObject);
    }
}
