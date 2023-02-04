using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Entity : MonoBehaviour
{
    public event System.Action OnDamageTaken;
    public event System.Action OnDeath;
    public event System.Action OnMove;

    public string Name { get { return _entityName; } }
    public string Description { get { return _description; } }
    public bool Blocking { get { return _blocking; } }
    public float Health { get { return _health; } }
    public float MaxHealth { get { return _maxHealth; } }

    [Header("Entity Settings")]
    [SerializeField] private string _entityName;
    [SerializeField] private string _description;
    [SerializeField] private float _health = 10f;
    [SerializeField] private float _maxHealth = 10f;
    [SerializeField] private bool _blocking;

    private Tile[] _occupiedTiles = new Tile[0];

    public virtual ActionButton.Definition[] GetActionButtons()
    {
        return null;
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
        _health -= damage;

        OnDamageTaken?.Invoke();

        if (_health <= 0)
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}
