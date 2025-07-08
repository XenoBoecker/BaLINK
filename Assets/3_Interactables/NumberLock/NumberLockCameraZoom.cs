using ECM.Components;
using ECM.Controllers;
using GameEvents;
using System.Collections;
using UnityEngine;

public class NumberLockCameraZoom : Interactable
{
    [SerializeField] private Camera lockCamera; // Reference to the main camera
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Transform playerZoomedOutPosition;

    [SerializeField] private float zoomTime = 0.5f; // Time to zoom in

    NumberLock numberLock;

    Camera mainCam;
    CharacterMovement playerController;
    MouseLook playerMouseLook;
    BaseFirstPersonController baseFirstPersonController;

    bool isZooming;
    bool zoomOutBuffered;
    bool isZoomedIn;

    InputSystem_Actions _inputActions;

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Enable(); // Enable the player input actions

        mainCam = Camera.main; // Get the main camera
        playerController = FindAnyObjectByType<CharacterMovement>(); // Find the player controller
        playerMouseLook = playerController.GetComponent<MouseLook>(); // Get the MouseLook component from the player controller
        baseFirstPersonController = playerController.GetComponent<BaseFirstPersonController>(); // Get the BaseFirstPersonController component
    }
    private void Start()
    {
        numberLock = GetComponent<NumberLock>(); // Get the NumberLock component from this GameObject
        numberLock.OnNumberLockOpened += ZoomOut; // Subscribe to the NumberLock opened event to zoom out
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable(); // Ensure the player input actions are enabled when the script is enabled
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable(); // Disable the player input actions when the script is disabled
    }

    private void Update()
    {
        if (_inputActions.Player.Cancel.WasPressedThisFrame())
        {
            if (isZoomedIn)
            {
                ZoomOut();
            }
            else if (isZooming)
            {
                zoomOutBuffered = true; // Buffer the zoom out action if currently zooming
            }
        }

        if (_inputActions.Player.Interact.WasPressedThisFrame())
        {
            // Raycast from mouse position to click on NumberSelector objects
            Ray ray = lockCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 100f)) // Adjust the distance as needed
            {
                NumberSelectorInteractable numberSelector = hitInfo.collider.GetComponent<NumberSelectorInteractable>();
                if (numberSelector != null)
                {
                    numberSelector.Interact(); // Call the interact method on the NumberSelector
                }
                NumberLockConfirmButton numberLockConfirmButton = hitInfo.collider.GetComponent<NumberLockConfirmButton>();
                if (numberLockConfirmButton != null)
                {
                    numberLockConfirmButton.Interact(); // Call the interact method on the confirm button
                }
            }
        }

        if (zoomOutBuffered)
        {
            ZoomOut(); // Perform the zoom out action
        }
    }

    override protected void Interact()
    {
        if(!numberLock.IsLocked) // Check if the number lock is not locked
        {
            return; // If the number lock is not locked, do nothing
        }
        ZoomIn();
    }
    public void ZoomIn()
    {
        if (isZooming)
        {
            return;
        }

        InteractedSuccessfully();

        StartCoroutine(ZoomInRoutine());
    }

    IEnumerator ZoomInRoutine()
    {
        isZooming = true;

        lockCamera.transform.position = mainCam.transform.position;
        lockCamera.transform.rotation = mainCam.transform.rotation;

        GetComponent<Collider>().enabled = false; // Disable the collider to prevent further interactions

        lockCamera.enabled = true; // Enable the lock camera
        mainCam.enabled = false; // Disable the main camera

        playerController.Pause(true);
        playerController.enabled = false; // Disable player movement
        playerMouseLook.enabled = false; // Disable mouse look to prevent camera movement
        baseFirstPersonController.enabled = false; // Disable the base first person controller


        for (float i = 0; i < zoomTime; i+= Time.deltaTime)
        {
            lockCamera.transform.position = Vector3.Lerp(lockCamera.transform.position, cameraPosition.position, i / zoomTime);
            lockCamera.transform.rotation = Quaternion.Lerp(lockCamera.transform.rotation, cameraPosition.rotation, i / zoomTime);
            yield return null; // Wait for the next frame
        }

        lockCamera.transform.position = cameraPosition.position; // Ensure final position is set
        lockCamera.transform.rotation = cameraPosition.rotation; // Ensure final rotation is set
        isZoomedIn = true;
        isZooming = false;

        Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
    }

    void ZoomOut()
    {
        if (isZooming)
        {
            return;
        }

        zoomOutBuffered = false;

        StartCoroutine(ZoomOutRoutine());
    }

    IEnumerator ZoomOutRoutine()
    {
        isZooming = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // Hide the cursor when zoomed out

        playerController.gameObject.transform.position = playerZoomedOutPosition.position; // Move the player to the zoomed out position
        playerController.gameObject.transform.rotation = playerZoomedOutPosition.rotation; // Set the player's rotation to the zoomed out position

        for (float i = 0; i < zoomTime; i += Time.deltaTime)
        {
            lockCamera.transform.position = Vector3.Lerp(
                cameraPosition.position,
                mainCam.transform.position, i / zoomTime);
            lockCamera.transform.rotation = Quaternion.Lerp(cameraPosition.rotation, mainCam.transform.rotation, i / zoomTime);
            yield return null; // Wait for the next frame
        }
        lockCamera.transform.position = mainCam.transform.position; // Ensure final position is set
        lockCamera.transform.rotation = mainCam.transform.rotation; // Ensure final rotation is set

        lockCamera.enabled = false; // Disable the lock camera
        mainCam.enabled = true; // Enable the main camera

        playerController.Pause(false);
        playerController.enabled = true; // Re-enable player movement
        playerMouseLook.enabled = true; // Re-enable mouse look
        baseFirstPersonController.enabled = true; // Re-enable the base first person controller

        GetComponent<Collider>().enabled = true; // Re-enable the collider for future interactions

        isZoomedIn = false;
        isZooming = false;
    }
}
