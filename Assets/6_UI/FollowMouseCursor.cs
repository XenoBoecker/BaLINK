using UnityEngine;
using UnityEngine.UI;

public class FollowMouseCursor : MonoBehaviour
{
    [SerializeField] private Sprite _numberLockCursor;
    [SerializeField] private Sprite _pauseCursor;
    [SerializeField] private Sprite _crashCursor;

    private GameObject _cursorObject;
    [SerializeField] private Image _cursorImage;

    public enum CursorType
    {
        None,
        NumberLock,
        Menu,
        Crash
    }


    private void Start()
    {
        _cursorObject = _cursorImage.gameObject;
        // Ensure the cursor starts as inactive
        SetCursor(CursorType.None);
    }
    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        _cursorObject.transform.position = mousePosition;
    }

    public void SetCursor(CursorType cursorType)
    {
        switch (cursorType)
        {
            case CursorType.None:
                _cursorObject.SetActive(false); // Hide the cursor object
                _cursorImage.sprite = null; // Set to no sprite
                break;
            case CursorType.NumberLock:
                _cursorObject.SetActive(true); // Show the cursor object
                _cursorImage.sprite = _numberLockCursor;
                break;
            case CursorType.Menu:
                _cursorObject.SetActive(true); // Show the cursor object
                _cursorImage.sprite = _pauseCursor;
                break;
            case CursorType.Crash:
                _cursorObject.SetActive(true); // Show the cursor object
                _cursorImage.sprite = _crashCursor;
                break;
            default:
                break;
        }
    }
}
