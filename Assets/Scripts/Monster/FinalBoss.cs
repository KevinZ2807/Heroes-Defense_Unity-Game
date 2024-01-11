using System.Data.Common;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FinalBoss : BaseEnemy
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float castingTime = 15f;
    [SerializeField] Transform enemyContainer;
    [SerializeField] private GameObject spawningEffect;
    private float timeBeforeCast;
    protected override void Start() {
        base.Start();
        AudioController.Ins.PlayBossFight();
    }

    protected override void Update() {
        timeBeforeCast += Time.deltaTime;
        base.Update();
        if (timeBeforeCast >= castingTime) {
            Cast();
            timeBeforeCast = 0f;
        }
    }

    private void Cast() {
        anim.SetTrigger("Casting");
        AudioController.Ins.PlayLightningSFX();
        int index = UnityEngine.Random.Range(0, enemyPrefabs.Length);
        int randomCount = UnityEngine.Random.Range(5, 10);
        GameObject prefabToSpawn = enemyPrefabs[index];
        LevelManager levelManager = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelManager>();
        EnemySpawner enemySpawner = GameObject.FindGameObjectWithTag("Level").GetComponent<EnemySpawner>();

        for (int i = 0; i < randomCount; i++) {
            int gateIndex = Random.Range(0, levelManager.listGate.Count());
            Instantiate(spawningEffect, levelManager.listGate[gateIndex].position, Quaternion.identity);
            enemySpawner.SpawnEnemy(prefabToSpawn, gateIndex);
            enemySpawner.AddEnemyAlive(1);
        }
        
    }
}
