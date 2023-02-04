using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event System.Action OnDamageTaken;

    public static Player Current { get; private set; }

    public float Health { get { return _health; } }
    public float MaxHealth { get { return _maxHealth; } }

    [SerializeField] private float _health = 100f;
    [SerializeField] private float _maxHealth = 100f;

    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Current = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        _health -= damage;
        OnDamageTaken?.Invoke();
    }
}
