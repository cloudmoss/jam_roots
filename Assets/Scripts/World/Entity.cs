using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Entity
{
    public event System.Action OnMove;

    public string Name { get { return _name; } }
    public bool Blocking { get { return _blocking; } }

    [SerializeField] private string _name;
    [SerializeField] private bool _blocking;

    private Tile[] _occupiedTiles = new Tile[0];

    public Entity(string name, bool blocking)
    {
        _name = name;
        _blocking = blocking;
    }

    public void ClearBlockers()
    {
        foreach (var tile in _occupiedTiles)
        {
            World.Current.GetTile(tile.Position).entities.Remove(this);
        }
    }

    public void SetOccupiedTiles(Tile[] tiles)
    {
        ClearBlockers();

        foreach (var tile in tiles)
        {
            World.Current.GetTile(tile.Position).entities.Add(this);
        }

        _occupiedTiles = tiles;

        OnMove?.Invoke();
    }
}
