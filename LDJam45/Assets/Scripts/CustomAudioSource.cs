using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class CustomAudioSource : MonoBehaviour { // Add this to all objects needing an audio source

    public AudioClip[] sounds = { null };
    private AudioSource source;
    public bool RandomizeAndPlayOnStart = false;
    void Start () {

        source = GetComponent<AudioSource> ();
        if (sounds.Length > 0) {
            if (sounds[0] == null) {
                sounds[0] = source.clip;
            }
        };
        if (AudioManager.instance != null) {
            AudioManager.instance.AddSource (source);
        }
        if (RandomizeAndPlayOnStart) {
            RandomizeAndPlay ();
        }
    }

    public void RandomizeSound () {
        if (sounds.Length > 0) {
            AudioClip randomClip = sounds[Random.Range (0, sounds.Length)];
            source.clip = randomClip;
        };
    }
    public void RandomizeAndPlay () {
        RandomizeSound ();
        Play ();
    }
    public void PausePlay (bool pause) {
        if (pause) {
            source.Pause ();
        } else {
            source.UnPause ();
        }
    }
    public void SetClip (AudioClip clip, bool loop) {
        source.clip = clip;
        source.loop = loop;
    }
    public void Play () {
        if (source.clip == null) {
            Debug.LogWarning ("No clip found!", source);
        } else {
            source.Play ();
        };
    }
    void OnDestroy () {
        if (AudioManager.instance != null) {
            AudioManager.instance.RemoveSource (source);
        };
    }
}