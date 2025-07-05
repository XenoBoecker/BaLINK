using UnityEngine;
using UnityEngine.VFX;

public class ShatterVFXTestTool : MonoBehaviour
{
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] VisualEffect _visualEffect;
    
    bool _isBroken = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _isBroken ^= true;
        }

        if (_isBroken)
        {
            _meshRenderer.enabled = false;
            _visualEffect.enabled = true;
            _visualEffect.SetVector3("Impact Position (World Space)", transform.position);
            _visualEffect.SendEvent("OnPlay");
        }
        else
        {
            _visualEffect.Reinit();
            _visualEffect.enabled = false;
            _meshRenderer.enabled = true;
        }
    }
}
