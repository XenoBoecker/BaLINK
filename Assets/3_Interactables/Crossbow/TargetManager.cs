using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private bool debug_enable_spawning_from_start;

    [SerializeField] private Hitable targetPrefab;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float spawnTimeDelay = 2f;
    [SerializeField] private int targetsToBeKilledCount = 6;
    public int TargetsToBeKilledCount => targetsToBeKilledCount; // Expose the count of targets to
    [SerializeField] private int targetCount = 10;

    int targetsSpawnedCount;
    int targetsDestroyedCount;
    public int TargetsDestroyedCount => targetsDestroyedCount;
    float spawnTimer;

    bool isSpawning;

    private void Update()
    {
        if(!isSpawning && !debug_enable_spawning_from_start) 
        { 
            return; 
        }

        if(targetsSpawnedCount >= targetCount)
        {
            return;
        }

        spawnTimer -= Time.deltaTime;

        if(spawnTimer < 0 )
        {
            spawnTimer = spawnTimeDelay;

            SpawnTarget();
        }
    }

    private void SpawnTarget()
    {
        Hitable newTarget = Instantiate(targetPrefab, spawnPoint);

        newTarget.OnDestroyed += IncreaseDestroyedTargetCount;
    }

    private void IncreaseDestroyedTargetCount(Hitable hitable)
    {
        hitable.OnDestroyed -= IncreaseDestroyedTargetCount;
        targetsDestroyedCount++;
    }

    public void StartSpawningTargets()
    {
        isSpawning = true;
    }
}
