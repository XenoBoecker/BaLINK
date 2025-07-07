using UnityEngine;

public class BombButtonFlashScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup correctButtonPanel;
    [SerializeField] private CanvasGroup wrongButtonPanel;

    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Bomb _bomb;

    private void Awake()
    {
        _bomb = FindAnyObjectByType<Bomb>();
    }
    private void Start()
    {
        _bomb.OnWrongButtonPressed += WrongButtonPressed;
        _bomb.OnCorrectButtonPressed += CorrectButtonPressed;

        correctButtonPanel.alpha = 0f; // Ensure the panel starts hidden
        wrongButtonPanel.alpha = 0f; // Ensure the panel starts hidden
    }

    private void CorrectButtonPressed()
    {
        FadeInAndOut(correctButtonPanel);
    }

    private void WrongButtonPressed()
    {
        FadeInAndOut(wrongButtonPanel);
    }

    void FadeInAndOut(CanvasGroup panel)
    {
        panel.gameObject.SetActive(true);
        StartCoroutine(FadeInAndOutRoutine(panel));
    }

    private System.Collections.IEnumerator FadeInAndOutRoutine(CanvasGroup panel)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            panel.alpha = fadeCurve.Evaluate(Mathf.Clamp01(elapsedTime / fadeDuration));
            yield return null;
        }
        panel.alpha = 0f; // Ensure it is fully faded out

        panel.gameObject.SetActive(false); // Disable the panel after fading out
    }

    private void OnDestroy()
    {
        _bomb.OnWrongButtonPressed -= WrongButtonPressed;
        _bomb.OnCorrectButtonPressed -= CorrectButtonPressed;
    }
}
