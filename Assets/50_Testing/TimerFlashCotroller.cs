using GameEvents;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TimerFlashCotroller : MonoBehaviour
{
    [SerializeField] private float _fadeInSpeed;
    [SerializeField] private float _startFadeOutDelay;
    [SerializeField] private float _fadeOutSpeed;

    private Image _flashImage;
    
    private void Awake()
    {
        _flashImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        ObjectEvents.OnWrongButtonPressed += WrongButtonPressed;
    }

    private void OnDisable()
    {
        ObjectEvents.OnWrongButtonPressed -= WrongButtonPressed;
    }

    private void WrongButtonPressed()
    {
        StartCoroutine(FlashAlpha());
    }

    private IEnumerator FlashAlpha()
    {
        _flashImage.color = new Color(_flashImage.color.r, _flashImage.color.g, _flashImage.color.b, 0);

        for (float i = 0; i < _fadeInSpeed; i += Time.deltaTime)
        {
            _flashImage.color = new Color(_flashImage.color.r, _flashImage.color.g, _flashImage.color.b, i / _fadeInSpeed);

            yield return null;
        }

        _flashImage.color = new Color(_flashImage.color.r, _flashImage.color.g, _flashImage.color.b, 1);
        yield return new WaitForSeconds(_startFadeOutDelay);

        for (float i = 0; i < _fadeOutSpeed; i += Time.deltaTime)
        {
            _flashImage.color = new Color(_flashImage.color.r, _flashImage.color.g, _flashImage.color.b, 1 - (i / _fadeOutSpeed));

            yield return null;
        }
    }
}
