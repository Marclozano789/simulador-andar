using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Ponlo en un objeto vacío FUERA del panel de ajustes (por ejemplo "SettingsManager").
public class SettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer mixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Panel")]
    public GameObject settingsPanel;
    public Button backButton;

    // Nombres EXACTOS de los parámetros expuestos en el AudioMixer
    const string MusicParam = "MusicVolume";
    const string SfxParam = "SFXVolume";

    const string MusicKey = "vol_music";
    const string SfxKey = "vol_sfx";

    public event System.Action Closed;
    public bool IsOpen => settingsPanel.activeSelf;

    void Start()
    {
        settingsPanel.SetActive(false);

        // Los sliders van de 0.0001 a 1
        musicSlider.minValue = 0.0001f; musicSlider.maxValue = 1f;
        sfxSlider.minValue = 0.0001f; sfxSlider.maxValue = 1f;

        // Cargar valores guardados (por defecto 0.8)
        float music = PlayerPrefs.GetFloat(MusicKey, 0.8f);
        float sfx = PlayerPrefs.GetFloat(SfxKey, 0.8f);

        musicSlider.SetValueWithoutNotify(music);
        sfxSlider.SetValueWithoutNotify(sfx);
        ApplyVolume(MusicParam, music);
        ApplyVolume(SfxParam, sfx);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        backButton.onClick.AddListener(Close);
    }

    public void Open() => settingsPanel.SetActive(true);

    public void Close()
    {
        settingsPanel.SetActive(false);
        PlayerPrefs.Save();
        Closed?.Invoke();
    }

    void SetMusicVolume(float value)
    {
        ApplyVolume(MusicParam, value);
        PlayerPrefs.SetFloat(MusicKey, value);
    }

    void SetSfxVolume(float value)
    {
        ApplyVolume(SfxParam, value);
        PlayerPrefs.SetFloat(SfxKey, value);
    }

    // Convierte 0-1 a decibelios (el mixer trabaja en dB, de -80 a 0)
    void ApplyVolume(string param, float value)
    {
        mixer.SetFloat(param, Mathf.Log10(value) * 20f);
    }
}
