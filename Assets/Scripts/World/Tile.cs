using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public struct Tile : IEquatable<Tile>
{
    public Vector2Int Position { get; private set; }

    public HashSet<Entity> entities;
    public TileDef Def { get; private set; }

    public Tile(Vector2Int position, TileDef def)
    {
        Position = position;
        this.Def = def;
        entities = new HashSet<Entity>();
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }


    public bool Equals(Tile other)
    {
        return other.Position == Position;
    }

}