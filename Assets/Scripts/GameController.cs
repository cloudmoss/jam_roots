using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private void Awake() 
    {
        TileLibrary.Init();
    }

    void Start()
    {
        Pathfinding.Init(World.Current);
    }
}
