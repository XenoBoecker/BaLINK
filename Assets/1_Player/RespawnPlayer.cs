using ECM.Components;
using ECM.Controllers;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    Vector3 respawnPosition; // Store the respawn position

    [SerializeField] private float killPlayerHeight = -10f; // Height at which the player is considered dead

    CharacterMovement _characterMovement;
    BaseFirstPersonController _baseFirstPersonController;

    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _baseFirstPersonController = GetComponent<BaseFirstPersonController>();
    }
    private void Start()
    {
        if (respawnPoint == null)
        {
            respawnPosition = transform.position; // Use current position if no respawn point is set
            Debug.LogError("Respawn point is not set!");
        }
        else
        {
            respawnPosition = respawnPoint.position; // Store the respawn position from the respawn point
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

        _characterMovement.Pause(true);
        _characterMovement.enabled = false; // Disable player movement
        _baseFirstPersonController.enabled = false; // Disable the base first person controller

        transform.position = respawnPosition;
        if (respawnPoint != null) transform.rotation = respawnPoint.rotation;


        _characterMovement.Pause(false);
        _characterMovement.enabled = true; // Disable player movement
        _baseFirstPersonController.enabled = true; // Disable the base first person controller



    }


}