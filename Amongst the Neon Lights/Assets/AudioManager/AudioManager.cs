using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    #region VAR_DECLORATION 

    public static AudioManager instance;

    public AudioMixerGroup mixerGroup;

    // Sounds 
    public Sound[] sounds;
    public Sound[] sfx;
    private Sound mainSong;

    private static bool instaStop = false;

    #endregion

    void Awake()
    {
        // Singleton
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }


        // Setting an audio player for each source in list
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = s.mixerGroup;
            s.source.playOnAwake = false;
        }

        foreach (Sound s in sfx)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = s.mixerGroup;
            s.source.playOnAwake = false;
        }
    }

    #region PLAYSOUNDS

    public void Play(string sound)
    {
        // Play a certain song
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        mainSong = s;

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    public void PlaySFX(string sound)
    {
        // Play a certain SFX
        Sound s = Array.Find(sfx, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    #endregion

    #region RECIEVE_DATA

    public bool isPlaying()
    {
        // returns wether a main song is playing
        if (mainSong == null) return false;
        return mainSong.source.isPlaying;
    }

    public float songTime()
    {
        // returns the time 
        return mainSong.source.time;
    }

    #endregion

    #region ALTER_MUSIC

    public void setSongTime(int time)
    {
        // Sets the time of a song
        mainSong.source.time = time;
    }

    public void InstaStopMusic()
    {
        // Stops all music without a fade out

        instaStop = true;

        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public void StopCurrentMusic()
    {
        // Stops all music with a fade out

        instaStop = false;

        foreach (Sound s in sounds)
        {
            StartCoroutine(FadeOut(s.source, 1f));
        }
    }

    public void SetSongSpeed(float multiplier)
    {
        // Sets the speed and pitch of the main song
        mainSong.source.pitch = multiplier;
    }

    #endregion

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        // Fades out a given audio track

        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            if (instaStop) yield break;

            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
