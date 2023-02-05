using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBehavior : EnemyBaseBehavior
{
    [SerializeField] private float _shootingRange = 5f;
    [SerializeField] private float _shootingCooldown = 3f;
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _velocity = 10f;
    [SerializeField] private float _inaccuracy = 4f;
    [SerializeField] private GameObject _bulletFX;
    [SerializeField] private float _sfxVolume = 0.3f;
    [SerializeField] private AudioClip[] _sfx;

    private float _shootingTimer = 0f;

    void Update()
    {
        if (Player.Current == null) return;

        EnemyBase.canMove = true;

        if (_shootingTimer > 0f)
        {
            _shootingTimer -= Time.deltaTime;
        }
        else
        {
            if (Vector2.Distance(transform.position, Player.Current.transform.position) < _shootingRange)
            {
                EnemyBase.canMove = false;

                BulletController.CreateBullet(_bulletFX, transform.position, (Player.Current.transform.position - transform.position).normalized * _velocity, _damage, _inaccuracy);
                _shootingTimer = _shootingCooldown;

                AudioController.PlaySfx(_sfx[Random.Range(0, _sfx.Length)], transform.position, _sfxVolume);
            }
            else
            {
                foreach (Tentacle tentacle in Player.Current.tentacles)
                {
                    if (Vector2.Distance(transform.position, tentacle.EndPosition) < _shootingRange)
                    {
                        EnemyBase.canMove = false;

                        BulletController.CreateBullet(_bulletFX, transform.position, (tentacle.EndPosition - (Vector2)transform.position).normalized * _velocity, _damage, _inaccuracy);
                        _shootingTimer = _shootingCooldown;
                        break;
                    }
                }
            }
        }
    }
}
