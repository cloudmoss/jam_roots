using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSettings : MonoBehaviour
{
    GameObject musicControl;

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
