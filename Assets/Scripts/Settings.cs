using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{

    public static float volume { get { return _volume; } set { _volume = value; PlayerPrefs.SetFloat("volume", value); } }
    public static bool gore { get { return _gore; } set { _gore = value; PlayerPrefs.SetInt("gore", value ? 1 : 0); } }

    private static float _volume = 1f;
    private static bool _gore = true;

    public static void LoadSettings()
    {
        if (PlayerPrefs.HasKey("volume"))
            _volume = PlayerPrefs.GetFloat("volume", 1f);
        if (PlayerPrefs.HasKey("gore"))
            _gore = PlayerPrefs.GetInt("gore", 1) == 1;
    }
}
