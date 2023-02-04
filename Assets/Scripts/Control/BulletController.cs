using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private class Bullet
    {
        public GameObject instance;
        public Transform transform;
        public Vector2 position;
        public Vector2 velocity;
        public float damage;
    }

    private class HitResult
    {
        public Bullet bullet;
        public Entity entity;
    }

    public static BulletController Current { get; private set; }

    private List<Bullet> _bullets = new List<Bullet>();
    private Task<Dictionary<Bullet, HitResult>> _hitResolverTask = null;

    private void Awake() {
        Current = this;
    }

    void Start()
    {
        StartCoroutine(HitResolver());
    }

    IEnumerator HitResolver()
    {
        while(true)
        {
            if (_hitResolverTask == null)
            {
                _hitResolverTask = Task.Run(() =>
                {
                    var results = new Dictionary<Bullet, HitResult>();

                    foreach (Bullet bullet in _bullets)
                    {
                        if (bullet == null) continue;
                        var playerhit = Player.Current.TestHit(bullet.position);
                        Entity entity = null;

                        if (!playerhit)
                        {
                            foreach (Tentacle tentacle in Player.Current.tentacles)
                            {
                                if (tentacle.TestHit(bullet.position))
                                {
                                    playerhit = true;
                                    entity = tentacle;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            entity = Player.Current;
                        }

                        if (playerhit)
                        {
                            results.Add(bullet, new HitResult()
                            {
                                bullet = bullet,
                                entity = entity
                            });
                        }
                    }

                    return results;
                });
            }
            else if (_hitResolverTask.IsCompleted)
            {
                var results = _hitResolverTask.Result;
                _hitResolverTask = null;

                foreach (KeyValuePair<Bullet, HitResult> result in results)
                {
                    result.Value.entity.DealDamage(result.Value.bullet.damage);
                    Destroy(result.Value.bullet.instance);
                    _bullets.Remove(result.Value.bullet);
                }
            }

            yield return null;
        }
    }

    void Update()
    {
        foreach (Bullet bullet in _bullets) 
        {
            bullet.position += bullet.velocity * Time.deltaTime;
            bullet.transform.position = bullet.position;
            bullet.transform.right = bullet.velocity.normalized;
        }
    }

    public static void CreateBullet(GameObject bulletFX, Vector2 position, Vector2 velocity, float damage, float angleDeviation)
    {
        var bullet = new Bullet()
        {
            instance = Instantiate(bulletFX, position, Quaternion.identity),
            velocity = velocity.RotateVector(Random.Range(-angleDeviation * 0.5f, angleDeviation * 0.5f)),
            damage = damage,
            position = position
        };

        bullet.transform = bullet.instance.transform;

        Current._bullets.Add(bullet);
    }
}
