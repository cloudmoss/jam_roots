using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckExtension : TentacleExtension
{
    [SerializeField] private float _suckInterval = 1f;
    [SerializeField] private float _range = 2f;

    private float _suckTimer;

    private void Update() {
        if (_suckTimer > 0f)
        {
            _suckTimer -= Time.deltaTime;
        }
        else
        {
            transform.up = _tentacle.TipFacingDirection;
            var piles = EnemyController.GetGoreInRange(transform.position, _range);
            if (piles.Length > 0)
            {
                var pile = piles[0];
                pile.Consume();
                _suckTimer = _suckInterval;
                StartCoroutine(Animate(pile.transform.position));
            }
        }
    }

    IEnumerator Animate(Vector2 target)
    {
        var t = 0f;
        var originalUp = transform.up;
        var dir = target - (Vector2)transform.position;

        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            var e = t < 0.5f ? EaseOutElastic(t * 2f, 0, 1, 1) : EaseOutElastic(1 - ((t - 0.5f) * 2f), 0, 1, 1);
            transform.localScale = Vector3.one + (Vector3.one * e * 0.5f);

            transform.up = Vector2.MoveTowards(originalUp, dir, Time.deltaTime * 180f);
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
