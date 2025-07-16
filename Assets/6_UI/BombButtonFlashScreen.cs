using UnityEngine;

public class BombButtonFlashScreen : MonoBehaviour
{
    [SerializeField] private Material timerTextMaterial;

    [SerializeField] private CanvasGroup correctButtonPanel;
    [SerializeField] private CanvasGroup wrongButtonPanel;

    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField] private float flashTimerSize = 1.1f;

    private Bomb _bomb;
    private BombUI _bombTimerTextUI;

    private void Awake()
    {
    }

    private void Start()
    {
        _bomb = FindAnyObjectByType<Bomb>();
        _bombTimerTextUI = FindAnyObjectByType<BombUI>();
        _bomb.OnWrongButtonPressed += WrongButtonPressed;
        _bomb.OnCorrectButtonPressed += CorrectButtonPressed;

        //correctButtonPanel.alpha = 0f; // Ensure the panel starts hidden
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
        if(panel == null)
        {
            yield break;
        }

        float elapsedTime = 0f;

        Color originalColor = timerTextMaterial.GetColor("_FaceColor");

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            panel.alpha = fadeCurve.Evaluate(Mathf.Clamp01(elapsedTime / fadeDuration));

            // flash white for the duration of the fade

            timerTextMaterial.SetColor("_FaceColor", Color.white * panel.alpha * 2 + originalColor * (1 - panel.alpha));

            // Scale the panel to create a flash effect
            float scale = Mathf.Lerp(1f, flashTimerSize, panel.alpha);

            _bombTimerTextUI.transform.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }
        panel.alpha = 0f; // Ensure it is fully faded out
        timerTextMaterial.SetColor("_FaceColor", originalColor); // Reset the color to original
        _bombTimerTextUI.transform.localScale = Vector3.one; // Reset the scale to original size

        panel.gameObject.SetActive(false); // Disable the panel after fading out
    }

    private void OnDestroy()
    {
        _bomb.OnWrongButtonPressed -= WrongButtonPressed;
        _bomb.OnCorrectButtonPressed -= CorrectButtonPressed;
    }
}
