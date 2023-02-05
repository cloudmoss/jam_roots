using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AudioController
{
    private static List<AudioSource> _audioSources;

    public static void PlaySfxUI(AudioClip clip, float volume = 1f, bool randomize = false, float pitchMultiplier = 1f)
    {
        volume *= Settings.volume;

        AudioSource a = GetAudioSource(randomize, pitchMultiplier, volume);

        a.spatialize = false;
        a.dopplerLevel = 0f;

        a.clip = clip;
        a.Play();
    }

    public static void PlaySfx(AudioClip clip, Vector2 position, float volume = 1f, bool randomize = true, float pitchMultiplier = 1f, bool spatialize = true, bool doppler = true)
    {
        volume *= Settings.volume;

        AudioSource a = GetAudioSource(randomize, pitchMultiplier, volume);

        a.spatialize = spatialize;
        a.dopplerLevel = doppler ? 1f : 0f;
        a.transform.position = new Vector3(position.x, position.y, -10);

        a.clip = clip;
        a.Play();
    }

    static AudioSource GetAudioSource(bool randomize, float pitchMultiplier, float volume)
    {
        if (_audioSources == null)
        {
            _audioSources = new List<AudioSource>();
            SceneManager.activeSceneChanged += delegate { _audioSources.Clear(); };
        }
        
        AudioSource a = null;

        if (_audioSources.Count > 0)
        {
            a = _audioSources.Find(s => !s.isPlaying);
        }

        if (a == null)
        {
            a = new GameObject("_AUDIOSOURCE_SPATIAL").AddComponent<AudioSource>();
            _audioSources.Add(a);
            a.spatialize = true;
            a.loop = false;
            a.spatialBlend = 1f;
            a.volume = 1;
            a.rolloffMode = AudioRolloffMode.Linear;
        }

        if (randomize)
        {
            a.pitch = pitchMultiplier + Random.Range(-0.125f, 0.125f);
            a.volume = volume + Random.Range(-0.125f, 0);
        }
        else
        {
            a.volume = volume;
            a.pitch = pitchMultiplier;
        }

        return a;
    }
}