using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileLibrary", menuName = "Game/TileLibrary")]
public class TileLibrary : ScriptableObject
{
    public static void Init()
    {
        if (_instance == null)
        {
            _instance = Resources.Load<TileLibrary>("TileLibrary");

            Debug.LogFormat("{0} tiles loaded",_instance._tiles.Count);
        }
    }

    public static TileDef GetTile(string name)
    {
        return _instance._tiles.Find(x => x.Name == name);
    }

    public static List<TileDef> Tiles {get { return _instance._tiles; } }

    private static TileLibrary _instance;

    [SerializeField] private List<TileDef> _tiles = new List<TileDef>();
}
