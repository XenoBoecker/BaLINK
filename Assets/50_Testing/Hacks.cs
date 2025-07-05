using ECM.Components;
using ECM.Controllers;
using GameEvents;
using UnityEngine;

public class Hacks : MonoBehaviour
{
    [SerializeField] private bool _enableHacks = false;

    [SerializeField] KeyCode blinkKey = KeyCode.Alpha1;

    CharacterMovement _characterMovement;
    BaseFirstPersonController _baseFirstPersonController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterMovement = FindAnyObjectByType<CharacterMovement>();
        _baseFirstPersonController = _characterMovement.GetComponent<BaseFirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_enableHacks) return;

        if (Input.GetKeyDown(blinkKey))
        {
            InputEvents.PlayerBlinked();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TeleportPlayer(new Vector3(0, 0, -1)); // Teleport down
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TeleportPlayer(new Vector3(0, 0, 1)); // Teleport up
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TeleportPlayer(new Vector3(-1, 0, 0)); // Teleport left
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TeleportPlayer(new Vector3(1, 0, 0)); // Teleport right
        }
    }

    void TeleportPlayer(Vector3 teleportVector)
    {
        _characterMovement.Pause(true);
        _characterMovement.enabled = false; // Disable player movement
        _baseFirstPersonController.enabled = false; // Disable the base first person controller

        _characterMovement.transform.position += teleportVector; // Teleport the player


        _characterMovement.Pause(false);
        _characterMovement.enabled = true; // Disable player movement
        _baseFirstPersonController.enabled = true; // Disable the base first person controller
    }
}
