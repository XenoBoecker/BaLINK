using UnityEngine;

public class MainMenuCursorSetup : MonoBehaviour
{
    [SerializeField] private Texture2D _cursorTexture;
    private void Awake()
    {
        Cursor.visible = true;
        Cursor.SetCursor(_cursorTexture, new Vector2(8, 8), CursorMode.ForceSoftware);
    }
}
