using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Current { get; private set; }
    
    public Tentacle[] tentacles { get; private set; }
    public Entity Entity { get; private set; }


    private SpriteRenderer _spriteRenderer;
    private Vector2 _position;

    private void Awake() {
        Entity = new Entity("Player", true);
        Entity.SetHealth(150f, 150f);
        _position = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Current = this;

        tentacles = GetComponentsInChildren<Tentacle>();
    }

    public void Damage(float damage)
    {
        Entity.DealDamage(damage);
    }

    public bool TestHit(Vector2 position)
    {
        return (Vector2.Distance(position, _position) < 1.5f);
    }
}
