using UnityEngine;

[ExecuteInEditMode]
public class BlinkAnimationController : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] float animationPercentage;
    [SerializeField] Material material;

    bool toggle;

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            toggle = true;
            material.SetFloat("_animationPercentage", animationPercentage);
            return;
        }

        if (toggle)
        {
            toggle = false;
            material.SetFloat("_animationPercentage", animationPercentage);
        }
    }

    private void OnDisable()
    {
        material.SetFloat("_animationPercentage", 0f);
    }
}
