using UnityEngine;
using UnityEngine.UI;

public class TargetCounterCanvas : MonoBehaviour
{
    [SerializeField] private Image[] targetCounterImages;
    [SerializeField] private float targetDestroyedAlphaValue = 0.3f;

    TargetManager _targetManager;

    private void Awake()
    {
        _targetManager = FindAnyObjectByType<TargetManager>();
    }

    private void Update()
    {
        if (_targetManager == null)
        {
            return;
        }

        int targetsDestroyed = _targetManager.TargetsDestroyedCount;

        for (int i = 0; i < targetCounterImages.Length; i++)
        {
            if (i < targetsDestroyed)
            {
                targetCounterImages[i].color = new Color(1f, 1f, 1f, targetDestroyedAlphaValue);
            }
            else
            {
                targetCounterImages[i].color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
}