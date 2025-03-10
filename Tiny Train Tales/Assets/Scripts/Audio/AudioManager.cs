using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [field:Header("Sound Effects")]
    [field:SerializeField] public AudioClip trainChuggingSFX { get; private set; }

    [Header("Settings")]
    [SerializeField] Slider sFXVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] float sFXVolume;
    [SerializeField] float musicVolume;

    [Header("Audio Pool Settings")]
    [SerializeField] int poolSize = 5;
    List<AudioSource> audioSourcePool = new List<AudioSource>();

    void Start()
    {
        SettingsSetUp();
        SetupAudioPool();

        PlayAudioClip(trainChuggingSFX);
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

        PlayerPrefs.Save();

        // Update volumes on all pooled AudioSources
        UpdateAudioSourceVolumes();
    }

    void UpdateAudioSourceVolumes()
    {
        foreach (AudioSource audioSource in audioSourcePool)
        {
            AudioFollow audioFollow = audioSource.gameObject.GetComponent<AudioFollow>();
            if (audioFollow != null)
            {
                // Use music volume if flagged as music; otherwise, use sFX volume.
                audioSource.volume = audioFollow.GetIsMusic ? musicVolume : sFXVolume;
            }
            else
            {
                audioSource.volume = sFXVolume;
            }
        }
    }

    /// Plays an audio clip from the pool and makes the AudioSource follow the provided target.
    /// If followTarget is null, the AudioSource will not follow any object.
    public void PlayAudioClip(AudioClip clip, bool isMusic = false)
    {
        if (clip == null)
            return;

        AudioSource audioSource = GetPooledAudioSource();
        if (audioSource == null) { return; }

        // Set the volume based on whether it's music or sfx.
        audioSource.volume = isMusic ? musicVolume : sFXVolume;
        // Enable 3D audio.

        AudioFollow audioFollow = audioSource.gameObject.GetComponent<AudioFollow>();
        if (audioFollow == null)
        {
            audioFollow = audioSource.gameObject.AddComponent<AudioFollow>();
        }
        audioFollow.GetIsMusic = isMusic;

        // Play the clip using PlayOneShot.
        audioSource.PlayOneShot(clip, audioSource.volume);
    }

    void SetupAudioPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = new GameObject("PooledAudioSource_" + i);
            go.transform.parent = transform;
            AudioSource source = go.AddComponent<AudioSource>();
            audioSourcePool.Add(source);
        }
    }

    // Returns an available AudioSource from the pool.
    // If all are busy, creates a new one, adds it to the pool, and returns it.
    AudioSource GetPooledAudioSource()
    {
        foreach (AudioSource source in audioSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        GameObject go = new GameObject("PooledAudioSource_New");
        go.transform.parent = transform;
        AudioSource newSource = go.AddComponent<AudioSource>();
        audioSourcePool.Add(newSource);
        return newSource;
    }
}
