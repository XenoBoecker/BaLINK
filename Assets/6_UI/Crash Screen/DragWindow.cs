using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour
{
    [SerializeField] private Transform spawnParent;
    [SerializeField] private float moveDistToSpawnplicate = 20f; // Distance to move before spawning a duplicate
    [SerializeField] private GameObject windowToDuplicate;
    [SerializeField] private int maxDuplicateCount = 50; // Limit the number of duplicates
    private RectTransform m_RectTransform;
    Vector3 lastPosition;

    int duplicateCount;

    bool followingMouse;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        lastPosition = m_RectTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (MouseHoverOverThis() && Input.GetMouseButtonDown(0))
        {
            followingMouse = true;
        }

        if (followingMouse)
        {
            if(Input.GetMouseButtonUp(0) || duplicateCount >= maxDuplicateCount)
            {
                followingMouse = false;
                return; // Stop following the mouse when the button is released
            }

            m_RectTransform.position = Input.mousePosition;

            if (Vector3.Distance(m_RectTransform.position, lastPosition) > moveDistToSpawnplicate)
            {
                // Spawn a duplicate of this window
                GameObject duplicate = Instantiate(windowToDuplicate, spawnParent);
                duplicate.GetComponent<RectTransform>().position = windowToDuplicate.GetComponent<RectTransform>().position;
                lastPosition = m_RectTransform.position; // Update last position to prevent multiple duplicates

                duplicateCount++;
            }
        }
    }

    private bool MouseHoverOverThis()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject == this.gameObject)
            {
                return true;
            }
            else
            {
                Debug.Log($"Raycast hit: {result.gameObject.name} but it is not a Button.");
            }
        }

        return false;
    }
}
