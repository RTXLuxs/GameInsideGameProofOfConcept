using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI (Optional)")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider brightnessSlider;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Post Processing")]
    [SerializeField] private Volume globalVolume;

    private ColorAdjustments colorAdjustments;

    private const string MasterVolumeKey = "MasterVolume";
    private const string BrightnessKey = "Brightness";

    private void Awake()
    {
        if (globalVolume == null)
            globalVolume = FindFirstObjectByType<Volume>();

        if (globalVolume != null)
            globalVolume.profile.TryGet(out colorAdjustments);
    }

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
        float brightness = PlayerPrefs.GetFloat(BrightnessKey, 0f);

        ApplyMasterVolume(volume);
        ApplyBrightness(brightness);

        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.SetValueWithoutNotify(volume);
            masterVolumeSlider.onValueChanged.AddListener(ApplyMasterVolume);
        }

        if (brightnessSlider != null)
        {
            brightnessSlider.SetValueWithoutNotify(brightness);
            brightnessSlider.onValueChanged.AddListener(ApplyBrightness);
        }
    }

    private void ApplyMasterVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;

        audioMixer.SetFloat("MasterVolume", dB);

        PlayerPrefs.SetFloat(MasterVolumeKey, value);
    }

    private void ApplyBrightness(float value)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = value;
        }

        PlayerPrefs.SetFloat(BrightnessKey, value);
    }

    private void OnDestroy()
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.RemoveListener(ApplyMasterVolume);

        if (brightnessSlider != null)
            brightnessSlider.onValueChanged.RemoveListener(ApplyBrightness);

        PlayerPrefs.Save();
    }
}