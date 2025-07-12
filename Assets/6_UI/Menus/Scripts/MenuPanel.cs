using UnityEngine;
using UnityEngine.Events;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private MenuPanel _escapePanel;

    [SerializeField] private UnityEvent _onPanelHidden;
    [SerializeField] private UnityEvent _onPanelShown;

    public MenuPanel EscapePanel => _escapePanel;

    public void Hide()
    {
        if (!gameObject.activeSelf) { return; }

        gameObject.SetActive(false);
        _onPanelHidden?.Invoke();
    }

    public void Show()
    {
        if (gameObject.activeSelf) { return; }
        
        gameObject.SetActive(true);
        _onPanelShown?.Invoke();
    }
}
