using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _dialogueVolumeSlider;

    private void OnEnable()
    {
        SaveSystem.LoadSaveData();
        
        _musicVolumeSlider.SetValueWithoutNotify(SaveSystem.Data.MusicVolume);
        _sfxVolumeSlider.SetValueWithoutNotify(SaveSystem.Data.SfxVolume);
        _dialogueVolumeSlider.SetValueWithoutNotify(SaveSystem.Data.DialogueVolume);
    }

    private void OnDisable()
    {
        SaveSystem.SaveSaveData();
    }

    public void SliderValueChanged()
    {
        SaveSystem.Data.MusicVolume = _musicVolumeSlider.value;
        SaveSystem.Data.SfxVolume = _sfxVolumeSlider.value;
        SaveSystem.Data.DialogueVolume = _dialogueVolumeSlider.value;

        SaveSystem.SaveSaveData();
    }
}
