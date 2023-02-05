using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    private float _volume = 0.35f;

    [SerializeField] private string _waveMusic;
    [SerializeField] private string _peaceMusic;

    private AudioSource audioSource;
    private AudioClip[] songList;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        songList = Resources.LoadAll<AudioClip>("Music");
    }

    private void Start()
    {
        ChangeSong(_peaceMusic);
        _volume *= Settings.volume;

        EnemyController.Current.OnWaveStart += () => ChangeSong(_waveMusic);
        EnemyController.Current.OnWaveCleared += () => ChangeSong(_peaceMusic);
    }

    private void Update() {
        if (Settings.volume != audioSource.volume * 0.4f)
        {
            audioSource.volume = Settings.volume * _volume;
        }
        audioSource.volume = Settings.volume * _volume;
    }

    public void ChangeSong(string state)
    {
        for (int i = 0; i < songList.Length; i++)
        {
            if (state == songList[i].name)
            {
                if (audioSource.clip == songList[i])
                {
                    return;
                } 
                else
                {
                    audioSource.volume = _volume;

                    if (audioSource.clip != null)
                        StartCoroutine(FadeMusicTo(1f, songList[i]));
                    else
                        audioSource.clip = songList[i];

                    if (!audioSource.isPlaying)
                        audioSource.Play();

                    return;
                }
            }
        }
    }

    IEnumerator FadeMusicTo(float fadeTime, AudioClip clip)
    {
        float startVolume = _volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * (Time.deltaTime / fadeTime);
            yield return null;
        }

        audioSource.clip = clip;
        audioSource.Play();

        while(audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * (Time.deltaTime / fadeTime);
            yield return null;
        }
    }
}
