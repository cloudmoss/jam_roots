using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitExtension : TentacleExtension
{
    [SerializeField] private float _cooldown = 2f;
    [SerializeField] private float _range = 10f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _projectileSpeed = 7f;
    [SerializeField] private AudioClip _sfx;
    [SerializeField] private Sprite[] _animation;
    [SerializeField] private GameObject _projectilePrefab;

    private float _timer;

    private void Awake() {
    }

    void Update()
    {
        if (_timer >= _cooldown)
        {
            var enemiesInRange = EnemyController.GetEnemiesInRange(transform.position, _range);
            transform.up = _tentacle.TipFacingDirection;

            if (enemiesInRange.Length > 0)
            {
                var projectile = Instantiate(_projectilePrefab, transform.position - (Vector3.forward * 5), Quaternion.identity);
                enemiesInRange[0].DealDamage(_damage);
                AudioController.PlaySfx(_sfx, transform.position, 1f);
                StartCoroutine(Animate(enemiesInRange[0].transform.position));
                StartCoroutine(ProjectileTrajectory(projectile.transform, enemiesInRange[0].transform.position));
                _timer = 0f;
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    IEnumerator ProjectileTrajectory(Transform projectile, Vector2 target)
    {
        var t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * _projectileSpeed;
            projectile.position = Vector3.Lerp(transform.position, target, t);

            yield return null;
        }
    
        Destroy(projectile.gameObject);
    }

    IEnumerator Animate(Vector2 target)
    {
        var t = 0f;
        var originalUp = transform.up;
        var dir = (target - (Vector2)transform.position).normalized;
        transform.localScale = Vector3.one * 1.2f;
        transform.up = dir;

        while (t < 1f)
        {
            transform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, t * t * t);
            t += Time.deltaTime * 15;
            _spriteRenderer.sprite = _animation[Mathf.Clamp(Mathf.FloorToInt(t * _animation.Length), 0, _animation.Length - 1)];
            yield return null;
        }
    }
}
