using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    // [SerializeField] AudioClip[] songList;
    private AudioSource audioSource;
    private AudioClip[] songList;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        songList = Resources.LoadAll<AudioClip>("Music");
    }
    private void Start()
    {
        ChangeSong("default");
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
                } else
                {
                    audioSource.clip = songList[i];
                }
            }
        }
    }
}
