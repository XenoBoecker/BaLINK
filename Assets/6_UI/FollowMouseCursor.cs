using UnityEngine;

public class FollowMouseCursor : MonoBehaviour
{
    [SerializeField] private GameObject _numberLockCursor;
    [SerializeField] private GameObject _pauseCursor;
    [SerializeField] private GameObject _crashCursor;
    [SerializeField] private GameObject _cursorParent;

    public enum CursorType
    {
        None,
        NumberLock,
        Pause,
        Crash
    }


    private void Start()
    {
        // Ensure the cursor starts as inactive
        SetCursor(CursorType.None);
    }
    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        _cursorParent.transform.position = mousePosition;
    }

    public void SetCursor(CursorType cursorType)
    {
        Debug.Log($"Setting cursor type: {cursorType}");

        _numberLockCursor.SetActive(cursorType == CursorType.NumberLock);
        _pauseCursor.SetActive(cursorType == CursorType.Pause);
        _crashCursor.SetActive(cursorType == CursorType.Crash);
    }
}
