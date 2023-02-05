using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    private bool paused = false;
    private int exitCount = 0;
    [SerializeField] Text exitText;

    void Start()
    {
        pausePanel.SetActive(false);
        Slider volumeControl = pausePanel.GetComponentInChildren<Slider>();
        volumeControl.value = PlayerPrefs.GetFloat("volume");

        volumeControl.onValueChanged.AddListener(delegate { PlayerPrefs.SetFloat("volume", volumeControl.value); Settings.LoadSettings(); });

        Toggle goreControl = pausePanel.GetComponentInChildren<Toggle>();
        if (PlayerPrefs.GetInt("gore") == 0)
        {
            goreControl.isOn = false;
        }
        else
        {
            goreControl.isOn = true;
        }

        goreControl.onValueChanged.AddListener(delegate { PlayerPrefs.SetInt("gore", goreControl.isOn ? 1 : 0); Settings.LoadSettings(); });
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            PauseGame();
        } else if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            ContinueGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        exitCount = 0;
        exitText.text = "Exit game";
        paused = true;
    }

    public void ContinueGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }

    public bool isPaused ()
    {
        return paused;
    }

    public void ExitToMenu()
    {
        if (exitCount == 0)
        {
            exitText.text = "Confirm exit";
            exitCount++;
        } else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
