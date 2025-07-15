using UnityEngine;

public class BombLED : MonoBehaviour
{
    [SerializeField] private Interactable interactable;

    [SerializeField] private GameObject ledLight;
    [SerializeField] private MeshRenderer ledMeshRenderer;
    [SerializeField] private Material ledOnMaterial;


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

        if (ledMeshRenderer != null && ledOnMaterial != null)
        {
            ledMeshRenderer.material = ledOnMaterial; // Change the material to indicate the LED is on
        }
    }

    private void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.OnInteracted -= TurnOnLEDLight; // Unsubscribe to avoid memory leaks
        }
    }
}
