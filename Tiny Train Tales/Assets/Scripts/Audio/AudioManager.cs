using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [field:Header("Sound Effects")]
    [field:SerializeField] public AudioClip[] trainAudios { get; private set; }

    [Header("Settings")]
    [SerializeField] Slider sFXVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] float sFXVolume;
    [SerializeField] float musicVolume;

    [Header("Audio Sources")]
    [SerializeField] AudioSource sFXAudioSource;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource trainAudioSource;

    void Start()
    {
        SettingsSetUp();
    }

    void SettingsSetUp()
    {
        sFXVolume = PlayerPrefs.HasKey("SFXVolume") ? PlayerPrefs.GetFloat("SFXVolume") : sFXVolume;
        sFXVolumeSlider.value = sFXVolume;

        musicVolume = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : musicVolume;
        musicVolumeSlider.value = musicVolume;
    }

    void Update()
    {
        ChangeSettings();
    }

    public void ChangeSettings()
    {
        sFXVolume = Mathf.Clamp(sFXVolumeSlider.value, 0, 1);
        PlayerPrefs.SetFloat("SFXVolume", sFXVolume);

        musicVolume = Mathf.Clamp(musicVolumeSlider.value, 0, 1);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        sFXAudioSource.volume = sFXVolume;
        musicAudioSource.volume = musicVolume;
        trainAudioSource.volume = sFXVolume;

        PlayerPrefs.Save();
    }

    public void PlayAudioClip(AudioClip clip, bool isMusic = false)
    {
        if (clip == null) return;

        if (isMusic)
        {
            musicAudioSource.PlayOneShot(clip);
        }
        else
        {
            sFXAudioSource.PlayOneShot(clip);
        }
    }

    public void PlayTrainAudio(AudioClip clip)
    {
        if (clip == null) return;

        trainAudioSource.clip = clip;
        trainAudioSource.Play();

        trainAudioSource.loop = clip == trainAudios[0];
    }
}
