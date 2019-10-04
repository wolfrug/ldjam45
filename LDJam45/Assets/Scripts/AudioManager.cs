using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    [SerializeField]
    private List<AudioSource> allSources = new List<AudioSource> { };
    public AudioSource musicSource;
    public float audioVolume = 1f;
    public float musicVolume = 1f;

    public GameObject muted;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy (this);
        }
    }

    public void ResetSources () {
        for (int i = 0; i < allSources.Count; i++) {
            if (allSources[i] == null) {
                allSources.RemoveAt (i);
            }
        }
    }
    public void AddSource (AudioSource source) { // Automagically happens on initialization of audio source
        if (!allSources.Contains (source)) {
            allSources.Add (source);
            source.volume = audioVolume;
        }
    }
    public void RemoveSource (AudioSource source) {
        if (allSources.Contains (source)) {
            allSources.Remove (source);
        }
    }

    public void SetAudioVolume (float volume) {
        foreach (AudioSource source in allSources) {
            if (source != null) {
                source.volume = volume;
            };
        }
        if (volume == 0f) {
            muted?.SetActive (true);
        } else {
            muted?.SetActive (false);
        }
        audioVolume = volume;
        // Also set the fungus volume, if applicable
        if (Fungus.FungusManager.Instance.MusicManager != null){
            Fungus.FungusManager.Instance.MusicManager.SetAudioVolume(volume, 0f, null);

        }
    }
    public void SetAudioVolumeReverse (float volume) {
        SetAudioVolume (1f - volume);
    }
    public void SetMusicVolumeReverse (float volume) {
        SetMusicVolume (1f - volume);
    }
    public void SetMusicVolume (float volume) {
        if (musicSource != null) {
            musicSource.volume = volume;
        }
        musicVolume = volume;
    }
}