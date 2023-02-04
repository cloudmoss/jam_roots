using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    // this code is just for testing and shit
    // will be deleted in the future

    private MusicChanger musicControl;
    private ResourceControl resourceControl;
    private Pause pause;

    void Start()
    {
        musicControl = GameObject.Find("Music").GetComponent<MusicChanger>();
        resourceControl = GameObject.Find("ResourceControl").GetComponent<ResourceControl>();
        pause = gameObject.GetComponent<Pause>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            musicControl.ChangeSong("Interval");
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            musicControl.ChangeSong("Traveler");
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!pause.isPaused())
            {
                pause.PauseGame();
            } else
            {
                pause.ContinueGame();
            }
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            gameObject.GetComponent<ScreenShake>().Shake(0.7f, 0.5f, 2f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            gameObject.GetComponent<BloodSplatter>().BloodSplat(mousePos);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            int randomHeight = Random.Range(4, 10);
            int randomwidth = Random.Range(3, 10);
            gameObject.GetComponent<BuildingGenerator>().Build(randomwidth, randomHeight, Vector3.zero);
        }

    }
}
