using UnityEngine;

[RequireComponent(typeof(Pickup))]
public class AutoPickupController : MonoBehaviour
{
    Pickup _pickup;

    private void Awake()
    {
        _pickup = GetComponent<Pickup>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            _pickup.TryInteract();
        }
    }
}
