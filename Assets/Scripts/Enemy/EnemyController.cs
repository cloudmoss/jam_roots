using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Current { get; private set; }
    public static List<EnemyBase> EnemyInstances { get; private set; } = new List<EnemyBase>();
    public static List<GorePile> GoreInstances { get; private set; } = new List<GorePile>();

    public int Wave { get; private set; } = 1;
    public float WaveTimer { get; private set; }

    [SerializeField] private List<EnemyBase> Enemies;

    void Awake()
    {
        Current = this;
    }

    IEnumerator SpawnWave()
    {
        float currency = Wave * 10;
        var possibleEnemies = Enemies.Where(e => e.DifficultyRating <= currency / 5f).ToList();
        var spawns = World.Current.Generator.SpawnPoints;

        while(currency > 0)
        {
            var enemy = possibleEnemies[Random.Range(0, Enemies.Count)];
            var spawnPoint = spawns[Random.Range(0, spawns.Length)];

            currency -= enemy.DifficultyRating;
            var inst = Instantiate(enemy, new Vector3(spawnPoint.x, spawnPoint.y, 0), Quaternion.identity);
            EnemyInstances.Add(inst);
            inst.OnDeath += () => {
                EnemyInstances.Remove(inst);
            };

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void RegisterGore(GorePile gore)
    {
        GoreInstances.Add(gore);
        gore.OnConsumed += () => GoreInstances.Remove(gore);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P))
            WaveTimer = 0;
    }

    private void Start() {
        SpawnWave();
        StartCoroutine(WaveCountdown());
    }

    IEnumerator WaveCountdown()
    {
        WaveTimer = 30f + Mathf.Min(110, Wave * 3f);

        while(WaveTimer > 0)
        {
            WaveTimer -= Time.deltaTime;
            yield return null;
        }

        Wave++;
        yield return StartCoroutine(SpawnWave());

        StartCoroutine(WaveCountdown());
    }

    public static EnemyBase[] GetEnemiesInRange(Vector2 position, float range)
    {
        return EnemyInstances.Where(e => Vector2.Distance(e.transform.position, position) <= range).ToArray();
    }

    public static GorePile[] GetGoreInRange(Vector2 position, float range)
    {
        return GoreInstances.Where(g => Vector2.Distance(g.transform.position, position) <= range).ToArray();
    }
}
