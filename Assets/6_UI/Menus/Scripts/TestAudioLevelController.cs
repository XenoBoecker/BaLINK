using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class TestAudioLevelController : MonoBehaviour
{
    [SerializeField] private AudioClip _testAudioClip;

    private InputSystem_Actions _inputActions;
    private AudioSource _source;
    private Slider _slider;

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();

        _source = GetComponent<AudioSource>();
        _source.clip = _testAudioClip;
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.Interact.started += ClickStarted;
        _inputActions.Player.Interact.canceled += ClickEnded;
    }

    private void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.Player.Interact.started -= ClickStarted;
        _inputActions.Player.Interact.canceled -= ClickEnded;
    }

    private void ClickStarted(InputAction.CallbackContext context)
    {
        _slider = null;
    }

    private void ClickEnded(InputAction.CallbackContext context)
    {
        if (_slider != null)
        {
            PlayVolumeTestAudio();
        }

        _slider = null;
    }

    private void PlayVolumeTestAudio()
    {
        if (_source == null || _source.clip == null) { return; }

        _source.volume = _slider.value;
        _source.Play();
    }
    
    public void SliderValueChanged(Slider slider)
    {
        _slider = slider;
    }
}
