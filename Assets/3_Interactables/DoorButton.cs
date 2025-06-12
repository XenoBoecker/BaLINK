using UnityEngine;

public class DoorButton : Interactable
{
    [SerializeField] private GameObject door; // Reference to the door GameObject

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact()
    {
        base.Interact();

        if (door != null)
        {
            // Toggle the door's active state when the button is pressed
            door.SetActive(!door.activeSelf);
        }
        else
        {
            Debug.LogWarning("Door reference is not set in the DoorButton script.");
        }
    }
}
