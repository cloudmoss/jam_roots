using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSettings : MonoBehaviour
{
    GameObject musicControl;

    // Start is called before the first frame update
    void Start()
    {
        musicControl = GameObject.Find("Music");
        float volume = PlayerPrefs.GetFloat("volume");
        musicControl.GetComponent<AudioSource>().volume = volume;
        int gore = PlayerPrefs.GetInt("gore");
        // however the gore is turned off
    }

    public void SaveSettings()
    {
        PlayerPrefs.Save();
    }

    public void ChangeVolume(float value)
    {
        PlayerPrefs.SetFloat("volume", value);
        SaveSettings();
        Settings.LoadSettings();
    }

    public void ToggleGore(bool goreOn)
    {
        int goreInt = 0;
        if (goreOn)
        {
            goreInt = 1;
        }
        PlayerPrefs.SetInt("gore", goreInt);
        SaveSettings();
        Settings.LoadSettings();
    }

}
