using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    [Header("References")]
    public AudioMixer masterMixer;
    public Slider volumeSlider;

    [Header("Settings")]
    [Range(0.001f, 1f)] public float minVolume = 0.001f;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", .5f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float sliderValue)
    {
        float volume = Mathf.Log10(sliderValue) * 20f;
        if (sliderValue < minVolume) volume = -80f;

        masterMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }

    void OnDestroy()
    {
        PlayerPrefs.Save();
    }
}
