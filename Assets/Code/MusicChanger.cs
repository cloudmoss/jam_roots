using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    public float volume = 0.35f;

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


        EnemyController.Current.OnWaveStart += () => ChangeSong(_waveMusic);
        EnemyController.Current.OnWaveCleared += () => ChangeSong(_peaceMusic);
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
                    audioSource.clip = songList[i];
                }
            }
        }
    }
}
