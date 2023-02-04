using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Current { get; private set; }

    public int Wave { get; private set; } = 1;
    public float WaveTimer { get; private set; }

    [SerializeField] private List<EnemyBase> Enemies;
    [SerializeField] private Vector2Int[] _spawnPoints;

    void Awake()
    {
        Current = this;
    }

    IEnumerator SpawnWave()
    {
        float currency = Wave * 10;
        var possibleEnemies = Enemies.Where(e => e.DifficultyRating <= currency / 5f).ToList();

        while(currency > 0)
        {
            var enemy = possibleEnemies[Random.Range(0, Enemies.Count)];
            //var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

            var pos = (Random.insideUnitCircle.normalized * 25) + (Vector2.one * 50);

            currency -= enemy.DifficultyRating;
            Instantiate(enemy, new Vector3(pos.x, pos.y, 0), Quaternion.identity);

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Start() {
        SpawnWave();
        StartCoroutine(WaveCountdown());
    }

    IEnumerator WaveCountdown()
    {
        WaveTimer = 10f;

        while(WaveTimer > 0)
        {
            WaveTimer -= Time.deltaTime;
            yield return null;
        }

        Wave++;
        yield return StartCoroutine(SpawnWave());
        StartCoroutine(WaveCountdown());
    }
}
