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
        // Solving according to algebraic form on (https://en.wikipedia.org/wiki/Line%E2%80%93plane_intersection)
        /* p0 */ Vector3 windowPos = transform.position;
        /* l0 */ Vector3 projectilePos = projectile.transform.position;
        /* l  */ Vector3 projectileDir = projectile.transform.forward;
        /* n  */ Vector3 windowNormal = transform.localToWorldMatrix * new Vector3(0, 0, 1);

        float d = Vector3.Dot(windowPos - projectilePos, windowNormal) / Vector3.Dot(projectileDir, windowNormal);

        return projectilePos + projectileDir * d;
    }
}
