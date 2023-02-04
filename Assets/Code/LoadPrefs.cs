using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPrefs : MonoBehaviour
{
    GameObject musicControl;
    void Start()
    {
        musicControl = GameObject.Find("Music");
        float volume = PlayerPrefs.GetFloat("volume");
        musicControl.GetComponent<AudioSource>().volume = volume;
        int gore = PlayerPrefs.GetInt("gore");
        // however the gore is turned off
    }
}
