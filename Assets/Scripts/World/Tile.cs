using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public struct Tile
{
    public Vector2Int Position { get; private set; }

    public TileDef Def { get; private set; }

    public Tile(Vector2Int position, TileDef def)
    {
        Position = position;
        this.Def = def;
    }
}