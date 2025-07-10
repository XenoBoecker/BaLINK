using UnityEngine;

public class TargetDoorOpener : MonoBehaviour
{
    [SerializeField] private DoorController door;

    TargetManager targetManager;

    private void Awake()
    {
        targetManager = FindAnyObjectByType<TargetManager>();
        if (targetManager == null)
        {
            Debug.LogError("TargetManager not found in the scene.");
        }
    }

    private void Update()
    {
        if (targetManager == null || targetManager.TargetsDestroyedCount < targetManager.TargetsToBeKilledCount)
        {
            return; // Do not open the door if the required targets are not destroyed
        }
        if (door != null && !door.IsOpen)
        {
            door.OpenDoor(); // Open the door when the required targets are destroyed
            Debug.Log("Door opened after destroying required targets.");
        }
    }
}
