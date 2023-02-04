using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Current { get; private set; }

    [SerializeField] private List<EnemyBase> Enemies;
    [SerializeField] private Vector2Int[] _spawnPoints;

    void Awake()
    {
        Current = this;
    }

    void Update()
    {
        
    }
}
