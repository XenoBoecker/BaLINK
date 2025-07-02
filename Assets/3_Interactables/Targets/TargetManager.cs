using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private bool debug_enable_spawning_from_start;

    [SerializeField] private Hitable targetPrefab;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float spawnTimeDelay = 2f;
    [SerializeField] private bool hoverRandomizeStartTime = true;
    public bool HoverRandomizeStartPosition => hoverRandomizeStartTime;

    [SerializeField] private int targetCount = 10;
    [SerializeField] private int targetsToBeKilledCount = 6;
    public int TargetsToBeKilledCount => targetsToBeKilledCount;

    [SerializeField] private AnimationCurve moveSpeedCurve;
    public AnimationCurve MoveSpeedCurve => moveSpeedCurve;
    [SerializeField] private AnimationCurve hoverSpeedCurve;
    public AnimationCurve HoverSpeedCurve => hoverSpeedCurve;

    [SerializeField] private float maxHoverDistanceToSpawnHeight = 3f;
    public float MaxHoverDistanceToSpawnHeight => maxHoverDistanceToSpawnHeight;
    [SerializeField] private float minDistanceToLastHoverHeight = 1.0f;
    public float MinDistanceToLastHoverHeight => minDistanceToLastHoverHeight;

    [Header("Slow Movement Settings")]
    [SerializeField] private float slowMoveSpeed = 0.5f;
    [SerializeField] private float slowHoverSpeed = 1f;

    [Header("Fast Movement Settings")]
    [SerializeField] private float fastMoveSpeed = 2f;
    [SerializeField] private float fastHoverSpeed = 2f;


    public float MoveSpeed => isMovingFast? fastMoveSpeed : slowMoveSpeed;
    public float HoverSpeed => isMovingFast? fastHoverSpeed : slowHoverSpeed;

    [SerializeField] private Vector3 moveDirection = Vector3.right;
    public Vector3 MoveDirection => moveDirection; // Expose the move direction to other scripts


    int targetsSpawnedCount;
    int targetsDestroyedCount;
    public int TargetsDestroyedCount => targetsDestroyedCount;
    float spawnTimer;

    bool isSpawning;
    bool isMovingFast = true;
    public bool IsMovingFast => isMovingFast; // Expose the moving speed to other scripts

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

    public void ToggleTargetMoveSpeed()
    {
        isMovingFast = !isMovingFast; // Toggle the moving speed between fast and slow
    }
}
