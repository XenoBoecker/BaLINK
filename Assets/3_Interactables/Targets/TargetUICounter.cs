using TMPro;
using UnityEngine;

public class TargetUICounter : MonoBehaviour
{
    private TargetManager targetManager;
    [SerializeField] private TMP_Text targetsDestroyedText;

    private void Awake()
    {
        targetManager = FindAnyObjectByType<TargetManager>();
    }
    private void Update()
    {
        if(targetsDestroyedText == null || targetManager == null)
        {
            return;
        }
        targetsDestroyedText.text = $"{targetManager.TargetsDestroyedCount}/{targetManager.TargetsToBeKilledCount}";
    }
}
