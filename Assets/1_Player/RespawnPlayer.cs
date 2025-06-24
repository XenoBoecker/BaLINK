using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    [SerializeField] private float killPlayerHeight = -10f; // Height at which the player is considered dead
    private void Start()
    {
        if (respawnPoint == null)
        {
            Debug.LogError("Respawn point is not set!");
        }
    }

    private void Update()
    {
        // Check if the player has fallen below the kill layer height
        if (transform.position.y < killPlayerHeight)
        {
            Respawn();
        }
    }
    public void Respawn()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;
        }
    }
}