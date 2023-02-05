using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject creditsPanel;
    [SerializeField] GameObject settingsPanel;

    private void Awake() {
        Settings.LoadSettings();
        Time.timeScale = 1f;
    }

    void Start()
    {
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        Slider volumeControl = settingsPanel.GetComponentInChildren<Slider>();
        volumeControl.value = Settings.volume;
        Toggle goreControl = settingsPanel.GetComponentInChildren<Toggle>();
        goreControl.isOn = Settings.gore;
    }

    // start game
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    // settings
    public void OpenSettings()
    {
        if (creditsPanel.activeInHierarchy == true)
        {
            CloseCredits();
        }

        settingsPanel.SetActive(!settingsPanel.activeInHierarchy);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ChangeVolume(float value)
    {
        PlayerPrefs.SetFloat("volume", value);
    }

    public void ToggleGore(bool goreOn)
    {
        int goreInt = 0;
        if(goreOn)
        {
            goreInt = 1;
        }
        PlayerPrefs.SetInt("gore", goreInt);
    }

    // credits
    public void OpenCredits()
    {
        if (settingsPanel.activeInHierarchy == true)
        {
            CloseSettings();
        }
        creditsPanel.SetActive(!creditsPanel.activeInHierarchy);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    // quit game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exit game");
    }
}
