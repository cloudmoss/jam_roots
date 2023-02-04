using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int DifficultyRating {get { return _difficultyRating; }}

    [SerializeField] private GameObject[] _deathPrefabs;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _meleeAttackRate = 1f;
    [SerializeField] private int _difficultyRating;

    private CircleCollider2D _collider;
    private float _meleeCooldown;
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = gameObject.AddComponent<CircleCollider2D>();
        _collider.radius = _spriteRenderer.bounds.extents.x;
    }

    void Update()
    {
        var playerPos = Player.Current.transform.position;

        if (Vector3.Distance(playerPos, transform.position) < 2f)
        {
            if (_meleeCooldown <= 0)
            {
                _meleeCooldown = 1f / _meleeAttackRate;
                Player.Current.Damage(_damage);
            }
            else
            {
                _meleeCooldown -= Time.deltaTime;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos, Time.deltaTime * _speed);
        }

        if (playerPos.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public void Kill()
    {
        Instantiate(_deathPrefabs[Random.Range(0, _deathPrefabs.Length)], transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));

        Destroy(gameObject);
    }
}
