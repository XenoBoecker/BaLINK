using UnityEngine;
using UnityEngine.VFX;

public class DestructibleWindow : Hitable
{
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] VisualEffect _visualEffect;

    public override void Hit(Transform projectile)
    {
        Debug.Log("Hit Position: " + projectile.position + "; my position: " + transform.position);

        Vector3 calculatedHitPosition = CalculateHitPosition(projectile);

        Debug.Log("Window has been hit!");
        _meshRenderer.enabled = false;
        _visualEffect.enabled = true;
        _visualEffect.SetVector3("Impact Position (World Space)", calculatedHitPosition);
        _visualEffect.SendEvent("OnPlay");
    }

    private Vector3 CalculateHitPosition(Transform projectile)
    {
        Vector3 projectileDir = projectile.transform.forward;

        float xDistance = Mathf.Abs(projectile.transform.position.x - transform.position.x);

        Vector3 impactpoint = projectile.transform.position - (xDistance / projectileDir.x * projectileDir);

        float x = transform.position.x;
        float y = projectile.transform.position.y;
        float z = projectile.transform.position.z;

        return new Vector3(x, y, z);
    }
}
