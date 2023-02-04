using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public event System.Action OnDeath;

    public int DifficultyRating {get { return _difficultyRating; }}
    public bool canMove;

    [SerializeField] private GameObject[] _deathPrefabs;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _meleeAttackRate = 1f;
    [SerializeField] private int _difficultyRating;

    private Coroutine _pathingCoroutine;
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

        if (_meleeAttackRate > 0f)
        {
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
                canMove = true;
            }
        }

        if (canMove && _pathingCoroutine == null)
        {
            PathTo(playerPos);
        }

        if (playerPos.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public void PathTo(Vector2 position)
    {
        _pathingCoroutine = StartCoroutine(MoveTo(position));
    }

    private IEnumerator MoveTo(Vector2 position)
    {
        var pathTask = Pathfinding.GetPathAsync(transform.position.ToVector2Int(), position.ToVector2Int(), 10000);

        while (!pathTask.IsCompleted)
        {
            yield return null;
        }

        var path = pathTask.Result;
        pathTask.Dispose();

        if (path == null)
        {
            _pathingCoroutine = null;
            yield break;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            var last = transform.position.ToVector2Int();
            for (int i = 0; i < path.Count; i++)
            {
                Debug.DrawLine(new Vector2(last.x, last.y), new Vector2(path[i].Position.x, path[i].Position.y), Color.yellow, 5);
                last = path[i].Position;
            }
        }

        while (path.Count > 0)
        {
            var nextPos = path[0].Position;

            while (Vector2.Distance(transform.position, nextPos) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, nextPos, _speed * Time.deltaTime);
                yield return null;
            }

            path.RemoveAt(0);
        }

        _pathingCoroutine = null;
    }

    public void Kill()
    {
        Instantiate(_deathPrefabs[Random.Range(0, _deathPrefabs.Length)], transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
