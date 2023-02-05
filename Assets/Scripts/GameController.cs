using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private void Awake() 
    {
        TileLibrary.Init();
        Time.timeScale = 1f;
    }

    void Start()
    {
        Pathfinding.Init(World.Current);
    }
}
