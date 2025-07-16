using ECM.Components;
using ECM.Controllers;
using GameEvents;
using System.Collections;
using UnityEngine;

public class Hacks : MonoBehaviour
{
    [SerializeField] private bool _enableHacks = false;

    [SerializeField] KeyCode blinkKey = KeyCode.Alpha1;

    [SerializeField] private float teleportDistance = 1f; // Distance to teleport the player
    [SerializeField] private float teleportDistAdder = 0.5f; // Multiplier for teleport distance when holding shift
    [SerializeField] private float teleportDistResetTime = 1f;
    float currentTeleportDistance = 1f; // Current teleport distance
    float teleportDistResetTimer = 0f; // Timer to reset teleport distance

    CharacterMovement _characterMovement;
    BaseFirstPersonController _baseFirstPersonController;
    PlayerFreezeController _playerFreezeController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterMovement = FindAnyObjectByType<CharacterMovement>();
        _baseFirstPersonController = _characterMovement.GetComponent<BaseFirstPersonController>();
        _playerFreezeController = _characterMovement.GetComponent<PlayerFreezeController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_enableHacks) return;

        if (Input.GetKeyDown(blinkKey))
        {
            InputEvents.PlayerBlinked();
        }

        teleportDistResetTimer -= Time.deltaTime;
        if(teleportDistResetTimer <= 0f)
        {
            currentTeleportDistance = teleportDistance; // Reset teleport distance
        }

        if(Input.GetKeyDown(KeyCode.V)) TeleportPlayer(Vector3.up * currentTeleportDistance); // Teleport up (vertical)

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TeleportPlayer(new Vector3(0, 0, -1) * currentTeleportDistance); // Teleport down
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TeleportPlayer(new Vector3(0, 0, 1) * currentTeleportDistance); // Teleport up
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TeleportPlayer(new Vector3(-1, 0, 0) * currentTeleportDistance); // Teleport left
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TeleportPlayer(new Vector3(1, 0, 0) * currentTeleportDistance); // Teleport right
        }
    }

    void TeleportPlayer(Vector3 teleportVector)
    {
        Debug.Log("Dist: " + currentTeleportDistance + " | Vector: " + teleportVector);

        
        StartCoroutine(TeleportRoutine(teleportVector));


        currentTeleportDistance += teleportDistAdder;
        teleportDistResetTimer = teleportDistResetTime; // Reset the timer
    }

    IEnumerator TeleportRoutine(Vector3 teleportVector)
    {
        _playerFreezeController.SetFreeze(true);
        _characterMovement.transform.position += teleportVector; // Teleport the player
        yield return new WaitForSeconds(0.1f); // Small delay to ensure the teleport is processed
        _playerFreezeController.SetFreeze(false);
    }
}
