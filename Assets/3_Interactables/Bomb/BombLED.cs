using UnityEngine;

public class BombLED : MonoBehaviour
{
    [SerializeField] private GameObject ledLight;
    [SerializeField] private Interactable interactable;

    private void Awake()
    {
        if (ledLight == null)
        {
            Debug.LogError("LED light reference is not set in BombLED.", this);
        }
        if (interactable == null)
        {
            Debug.LogError("Interactable reference is not set in BombLED.", this);
        }

        interactable.OnInteracted += TurnOnLEDLight;

        ledLight.SetActive(false); // Ensure the LED light is off at start
    }

    private void TurnOnLEDLight()
    {
        ledLight.SetActive(true);
    }
}
