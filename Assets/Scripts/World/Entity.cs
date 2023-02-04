using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Entity
{
    public event System.Action OnDamageTaken;
    public event System.Action OnMove;

    public string Name { get { return _name; } }
    public bool Blocking { get { return _blocking; } }
    public float Health { get { return _health; } }
    public float MaxHealth { get { return _maxHealth; } }

    [SerializeField] private float _health = 100f;
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private string _name;
    [SerializeField] private bool _blocking;

    private Tile[] _occupiedTiles = new Tile[0];

    public Entity(string name, bool blocking)
    {
        _name = name;
        _blocking = blocking;
    }

    public void SetHealth(float health)
    {
        _health = health;
    }

    public void SetHealth(float health, float maxHealth)
    {
        _health = health;
        _maxHealth = maxHealth;
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

    public void DealDamage(float damage)
    {
        OnDamageTaken?.Invoke();
    }
}
