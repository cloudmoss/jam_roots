using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeExtension : TentacleExtension
{
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _range = 1f;
    [SerializeField] private float _cooldown = 1f;
    [SerializeField] private AudioClip[] _hitSounds;


    private float _timer = 0f;

    void Update()
    {
        if (_timer >= _cooldown)
        {
            var enemiesInRange = EnemyController.GetEnemiesInRange(transform.position, _range);
            transform.up = _tentacle.TipFacingDirection;

            if (enemiesInRange.Length > 0)
            {
                enemiesInRange[0].DealDamage(_damage);
                AudioController.PlaySfx(_hitSounds[Random.Range(0, _hitSounds.Length)], transform.position, 1f);
                StartCoroutine(Animate(enemiesInRange[0].transform.position));
                _timer = 0f;
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    IEnumerator Animate(Vector2 target)
    {
        var t = 0f;
        var originalUp = transform.up;

        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            var e = t < 0.5f ? EaseOutElastic(t * 2f, 0, 1, 1) : EaseOutElastic(1 - ((t - 0.5f) * 2f), 0, 1, 1);
            transform.localScale = Vector3.one + (Vector3.one * e * 0.5f);

            var dir = target - (Vector2)transform.position;
            transform.up = Vector2.Lerp(originalUp, dir, e);

            yield return null;
        }
    }    
    
    // Elastic ease out
    // t = current time
    // b = start value
    // c = change in value
    // d = duration
    float EaseOutElastic(float t, float b, float c, float d)
    {
        float s = 1.70158f;
        float p = 0;
        float a = c;

        if (t == 0) return b;
        if ((t /= d) == 1) return b + c;
        if (p == 0) p = d * 0.3f;
        if (a < Mathf.Abs(c))
        {
            a = c;
            s = p / 4;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(c / a);
        }

        return a * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) + c + b;
    }
}
