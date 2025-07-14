using UnityEngine;

public class SwapSkyboxController : MonoBehaviour
{
    [SerializeField] private Material _targetSkybox;
    private Material _originalSkybox;
    private GameObject _exteriorElements;

    private void Awake()
    {
        _exteriorElements = GameObject.FindGameObjectWithTag("Exterior");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _exteriorElements.SetActive(false);
            _originalSkybox = RenderSettings.skybox;
            RenderSettings.skybox = _targetSkybox;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _exteriorElements.SetActive(true);
            RenderSettings.skybox = _originalSkybox;
        }
    }
}
