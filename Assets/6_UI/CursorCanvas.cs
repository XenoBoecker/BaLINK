using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class CursorCanvas : MonoBehaviour
{
    [SerializeField] private Image fixedCursorImage, movingCursorImage;
    [SerializeField] private Texture2D _defaultCursor;
    [SerializeField] private Sprite normalSprite, canInteractSprite;

    [SerializeField] FollowMouseCursor movingCursor;

    Canvas cursorCanvas;
    PlayerInteractor playerInteractor;
    PauseMenuController pauseMenu;
    CrashScreenController crashScreen;
    

    int startCanvasSortingOrder;

    bool gameHasCrashed = false;
    bool isInThanksScreen = false;

    private void Awake()
    {
        cursorCanvas = GetComponent<Canvas>();
        startCanvasSortingOrder = cursorCanvas.sortingOrder;
    }

    private void Start()
    {
        Cursor.SetCursor(_defaultCursor, new Vector2(8, 8), CursorMode.ForceSoftware);

        playerInteractor = FindAnyObjectByType<PlayerInteractor>();
        pauseMenu = FindAnyObjectByType<PauseMenuController>();
        crashScreen = FindAnyObjectByType<CrashScreenController>();
    }

    private void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        Cursor.visible = false;
        if (isInThanksScreen)
        {
            SetMovingCursor(FollowMouseCursor.CursorType.Menu);
            ShowHoverInteractUI(IsMouseOverUIButton());
        }
        else if (GameHasCrashed())
        {
            Debug.Log("Game has crashed, showing crash cursor.");
            SetMovingCursor(FollowMouseCursor.CursorType.Crash);
            return;
        }
        else if (pauseMenu != null && pauseMenu.IsPaused)
        {
            SetMovingCursor(FollowMouseCursor.CursorType.Menu);
            ShowHoverInteractUI(IsMouseOverUIButton());
            return;
        }
        else if (WeaponEquipped())
        {
            SetMovingCursor(FollowMouseCursor.CursorType.None);
            fixedCursorImage.gameObject.SetActive(false);
            return;
        }
        else if (PlayerIsInMinigame())
        {
            SetMovingCursor(FollowMouseCursor.CursorType.NumberLock);
            return;
        }

        if(normalSprite == null || canInteractSprite == null)
        {
            Debug.LogWarning("Cursor sprites are not assigned in the CursorCanvas script.");
            return;
        }

        if (playerInteractor == null) // player in main menu
        {
            SetMovingCursor(FollowMouseCursor.CursorType.Menu);

            ShowHoverInteractUI(IsMouseOverUIButton());
        }
        else
        {
            SetMovingCursor(FollowMouseCursor.CursorType.None);

            ShowHoverInteractUI(playerInteractor.IsMouseHoverOverInteractable());
        }

    }

    void SetMovingCursor(FollowMouseCursor.CursorType cursorType)
    {
        if (movingCursor == null)
        {
            Debug.LogError("Moving cursor is not assigned in the CursorCanvas script.");
            return;
        }
        if(cursorType == FollowMouseCursor.CursorType.Crash)
        {
            movingCursorImage.rectTransform.Rotate(new Vector3(0,0,-1), crashScreen.CursorRotateSpeed * Time.deltaTime);
            cursorCanvas.sortingOrder = 200;
        }
        else
        {
            movingCursorImage.rectTransform.rotation = Quaternion.identity; // Reset rotation for other cursor types
            cursorCanvas.sortingOrder = startCanvasSortingOrder;
        }

        if (cursorType == FollowMouseCursor.CursorType.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            movingCursorImage.gameObject.SetActive(false);
            fixedCursorImage.gameObject.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            movingCursorImage.gameObject.SetActive(true);
            fixedCursorImage.gameObject.SetActive(false);
        }

        movingCursor.SetCursor(cursorType);
    }

    void ShowHoverInteractUI(bool v)
    {
        if (v)
        {
            fixedCursorImage.sprite = canInteractSprite;
            movingCursorImage.sprite = canInteractSprite;
        }
        else
        {
            fixedCursorImage.sprite = normalSprite;
            movingCursorImage.sprite = normalSprite;
        }
    }

    private bool WeaponEquipped()
    {
        if (playerInteractor == null) return false;
        return playerInteractor.IsItemEquipped(EquippedItem.ItemType.Weapon);
    }

    private bool PlayerIsInMinigame()
    {
        return Camera.main == null || !Camera.main.enabled;
    }

    bool IsMouseOverUIButton()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<Button>() != null)
            {
                return true;
            }
            else
            {
                Debug.Log($"Raycast hit: {result.gameObject.name} but it is not a Button.");
            }
        }

        return false;
        // UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() || UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1);
    }

    private bool GameHasCrashed()
    {
        if (crashScreen == null) return false;

        if (crashScreen.MenuIsVisible)
        {
            gameHasCrashed = true;
        }
        else
        {
            if (gameHasCrashed)
            {
                isInThanksScreen = true;
            }
        }

        return gameHasCrashed;
    }
}
