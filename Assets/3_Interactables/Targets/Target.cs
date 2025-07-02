using System;
using UnityEngine;
using UnityEngine.Timeline;

[RequireComponent(typeof(Hitable))]
public class Target : MonoBehaviour
{
    [SerializeField] private Transform pathParent;
    [SerializeField] private bool returnPathBackwards = true;

    Transform[] path;
    int lastPathPointIndex = 0, currentPathPointIndex = 1;
    float lastHoverGoalHeight, currentHoverGoalHeight;

    TargetManager targetManager;
    Hitable hitable;

    float spawnHeight;

    bool movementDisabled;

    // path to follow; bool loop or back and forth; up and down also slow and fast speeds and heights; random in game when change from up to down and back
    // base height, thresholds for up and down; random next goal height in range of thresholds
    // smoothing for stopping (curve speed from start to goalpos)

    private void Awake()
    {
        targetManager = FindAnyObjectByType<TargetManager>();
        if (targetManager == null)
        {
            Debug.LogError("TargetManager not found in the scene. Please ensure it is present.", this);
            return;
        }

        hitable = GetComponent<Hitable>();
        hitable.OnHit += DisableMovement;

        if (targetManager.HoverRandomizeStartPosition)
        {
            transform.Translate(Vector3.up * UnityEngine.Random.Range(-1f, 1f));
        }

        path = new Transform[pathParent.childCount];

        for (int i = 0; i < pathParent.childCount; i++)
        {
            path[i] = pathParent.GetChild(i);
        }
    }

    private void DisableMovement()
    {
        movementDisabled = true;
    }

    private void Start()
    {
        spawnHeight = transform.position.y; // Store the initial height of the target
        lastHoverGoalHeight = spawnHeight;
        GetNextHoverPoint();
    }

    private void Update()
    {
        if(movementDisabled)
        {
            return;
        }

        if(path.Length == 0)
        {
            transform.Translate(targetManager.MoveDirection * targetManager.MoveSpeed * Time.deltaTime);
        }
        else
        {
            MoveAlongPath();
        }

        HoverUpAndDown();
    }

    private void MoveAlongPath()
    {
        Transform lastPathPoint = path[lastPathPointIndex];
        Transform currentPathPoint = path[currentPathPointIndex];

        Vector3 noHeightPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 noHeightLastPathPoint = new Vector3(lastPathPoint.position.x, 0, lastPathPoint.position.z);
        Vector3 noHeightCurrentPathPoint = new Vector3(currentPathPoint.position.x, 0, currentPathPoint.position.z);

        float distPercentage = Vector3.Distance(noHeightPos, noHeightLastPathPoint) / Vector3.Distance(noHeightLastPathPoint, noHeightCurrentPathPoint);
        print(distPercentage);
        float moveSpeed = targetManager.MoveSpeedCurve.Evaluate(distPercentage) * targetManager.MoveSpeed;
        if(moveSpeed < 0.1f)
        {
            moveSpeed = 0.1f; // Ensure a minimum speed to avoid getting stuck
        }
        Vector3 moveDir = (noHeightCurrentPathPoint - noHeightPos).normalized;
        transform.Translate(moveSpeed * Time.deltaTime * moveDir);

        if(Vector3.Distance(noHeightPos, currentPathPoint.position) < 0.2f)
        {
            GetNextPathPoint();
        }
    }

    private void GetNextPathPoint()
    {
        lastPathPointIndex = currentPathPointIndex;

        if (returnPathBackwards)
        {
            if((currentPathPointIndex < lastPathPointIndex && currentPathPointIndex != 0) || currentPathPointIndex == path.Length - 1)
            {
                currentPathPointIndex--;
            }
            else
            {
                currentPathPointIndex++;
            }
        }

        else
        {
            currentPathPointIndex++;
            if (currentPathPointIndex >= path.Length)
            {
                currentPathPointIndex = 0;
            }
        }
    }

    private void HoverUpAndDown()
    {
        float distPercentage = Mathf.Abs(transform.position.y - lastHoverGoalHeight) / Mathf.Abs(lastHoverGoalHeight - currentHoverGoalHeight);
        float hoverSpeed = targetManager.HoverSpeedCurve.Evaluate(distPercentage) * targetManager.HoverSpeed;
        if(hoverSpeed < 0.1f)
        {
            hoverSpeed = 0.1f; // Ensure a minimum speed to avoid getting stuck
        }
        transform.Translate(Vector3.up * Mathf.Sign(currentHoverGoalHeight - lastHoverGoalHeight) * hoverSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.position.y - currentHoverGoalHeight) < 0.2f)
        {
            GetNextHoverPoint();
        }
    }

    void GetNextHoverPoint()
    {
        int tryCount = 0;
        lastHoverGoalHeight = currentHoverGoalHeight;
        float newHoverGoalHeight = currentHoverGoalHeight;

        while (Mathf.Abs(newHoverGoalHeight - lastHoverGoalHeight) < targetManager.MinDistanceToLastHoverHeight && tryCount < 10)
        {
            newHoverGoalHeight = spawnHeight + UnityEngine.Random.Range(-targetManager.MaxHoverDistanceToSpawnHeight, targetManager.MaxHoverDistanceToSpawnHeight);
            tryCount++;
        }

        currentHoverGoalHeight = newHoverGoalHeight;

        Debug.Log("Next hover goal height: " + currentHoverGoalHeight, this);
    }
}
