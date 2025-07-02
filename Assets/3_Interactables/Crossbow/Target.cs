using System;
using UnityEngine;

[RequireComponent(typeof(Hitable))]
public class Target : MonoBehaviour
{
    [SerializeField] private float slowMoveSpeed = 0.5f;
    [SerializeField] private float fastMoveSpeed = 2f;


    [SerializeField] private float hoverSpeed = 1f; // Speed of the up and down movement
    [SerializeField] private float hoverHeight = 0.5f; // Height of the hover movement
    [SerializeField] private bool hoverRandomizeStartTime = true;

    [SerializeField] private Vector3 moveDirection = Vector3.right;

    Hitable hitable;

    float hoverRandomStartTime;
    float baseHeight;
    bool isMovingFast = false;

    bool movementDisabled;

    private void Awake()
    {
        hitable = GetComponent<Hitable>();
        hitable.OnHit += DisableMovement;

        if (hoverRandomizeStartTime)
        {
            hoverRandomStartTime = UnityEngine.Random.Range(0f, 10f); // Randomize the start time for hovering
        }
    }

    private void DisableMovement()
    {
        movementDisabled = true;
    }

    private void Start()
    {
        baseHeight = transform.position.y; // Store the initial height of the target
    }

    private void Update()
    {
        if(movementDisabled)
        {
            return;
        }

        float moveSpeed = isMovingFast ? fastMoveSpeed : slowMoveSpeed;

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        HoverUpAndDown();
    }

    private void HoverUpAndDown()
    {
        float newY = baseHeight + Mathf.Sin(hoverRandomStartTime + Time.time * hoverSpeed) * hoverHeight;
        Vector3 newPosition = transform.position;
        newPosition.y = newY;
        transform.position = newPosition;
    }

    public void ToggleMovementSpeed()
    {
        isMovingFast = !isMovingFast; // Toggle between slow and fast movement
        Debug.Log($"Target speed toggled to {(isMovingFast ? "fast" : "slow")}.");
    }
}
