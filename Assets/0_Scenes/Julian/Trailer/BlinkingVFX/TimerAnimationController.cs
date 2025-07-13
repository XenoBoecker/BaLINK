using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TimerAnimationController : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] private int _timeInSeconds = 300;
    [SerializeField] TMP_Text _textComponent;

    bool toggle;

    private void Awake()
    {
        _textComponent = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (active)
        {
            toggle = true;
            _textComponent.text = TimeToText(_timeInSeconds);
            return;
        }

        if (toggle)
        {
            toggle = false;
            _textComponent.text = TimeToText(_timeInSeconds);
        }
    }

    private void OnDisable()
    {
        _textComponent.text = TimeToText(_timeInSeconds);
    }

    private static string TimeToText(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;

        string secondText = "";

        if (seconds < 10)
        {
            secondText = "0";
        }

        secondText += seconds;

        return "0" + minutes + " : " + secondText;
    }
}
