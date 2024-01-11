using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public EnemySpawn[] enemySpawns;
    }

    [System.Serializable]
    public class EnemySpawn
    {
        public GameObject enemyPrefab;
        public int count;
    }

    [Header("References")]
    [SerializeField] Transform enemyContainer;

    [Header("Attributes")]
    [SerializeField] float timeToPlaceHero = 10f;
    [SerializeField] float timeToCombat = 10f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();
    public static event Action<float, bool, int> SetCountEvent = delegate { };
    public static event Action OnComplete = delegate { };
    public static float timeSinceWaveStart;
    public static bool IsSpawning;

    public Wave[] waves;
    public int currentWave = 0;

    private int cntEnemies=0; 
    private int enemiesAlive; // Keep track of how many enemy's alive

    private void Awake() {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start() {
        //StartCoroutine(StartWave());
        StartCoroutine(SpawnWaves());
    }
    IEnumerator SpawnWaves()
    {
        while (currentWave < waves.Length)
        {
            IsSpawning = false;
            SetCountEvent.Invoke(timeToPlaceHero, false, 0);
            yield return new WaitForSeconds(timeToPlaceHero);
            SetCountEvent.Invoke(timeToCombat, true, currentWave);
            IsSpawning = true;
            if (currentWave == 4) IsSpawning = false;
            GameEngine.Ins.BFSPath();
            yield return StartCoroutine(SpawnWave(waves[currentWave])); // Bat dau wave moi
            yield return new WaitUntil(() => enemiesAlive == 0);
             // Doi thoi gian spawn hero
            currentWave++;
        }
        OnComplete.Invoke();
    }

    public void SpawnEnemy(GameObject enemePrefab) {
        LevelManager levelManager = GetComponent<LevelManager>();
        int gateIndex = Random.Range(0, levelManager.listGate.Count());
        GameObject enemy = Instantiate(enemePrefab, levelManager.listGate[gateIndex].position, Quaternion.identity);
        enemy.transform.SetParent(enemyContainer);
        enemy.GetComponent<BaseEnemy>().SetGateIndex(gateIndex);
        enemy.GetComponent<BaseEnemy>().cnt = cntEnemies;
    }

    public void SpawnEnemy(GameObject enemePrefab, int gateIndex) { // For boss use
        LevelManager levelManager = GetComponent<LevelManager>();
        GameObject enemy = Instantiate(enemePrefab, levelManager.listGate[gateIndex].position, Quaternion.identity);
        enemy.transform.SetParent(enemyContainer);
        enemy.GetComponent<BaseEnemy>().SetGateIndex(gateIndex);
    }
    private void EnemyDestroyed() {
        enemiesAlive--;
    }

    IEnumerator SpawnWave(Wave wave)
    {
        foreach (var enemySpawn in wave.enemySpawns) {
            for (int i = 0; i < enemySpawn.count; i++) {
                SpawnEnemy(enemySpawn.enemyPrefab);
                cntEnemies++;
                enemiesAlive++;
                //Instantiate(enemySpawn.enemyPrefab, GetSpawnPosition(), Quaternion.identity);
                yield return new WaitForSeconds(0.1f); // Spawn delay
            }
        }
    }

    public void AddEnemyAlive(int moreEnemy) {
        enemiesAlive += moreEnemy;
    }
}
