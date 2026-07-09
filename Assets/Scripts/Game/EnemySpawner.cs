using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    float timer;
    float spawnInterval = 1.5f;
   [SerializeField] private EnemySpawnArea enemySpawnArea;

    GameConfigSO cfg;

    private void Start()
    {
        cfg = ConfigManager.Instance.GameConfig;
        spawnInterval = cfg.enemySpawnInterval;
}

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = ObjectPool.Instance.GetEnemyNormal();

        if (enemySpawnArea)
        {
            enemy.transform.position = enemySpawnArea.GetRandomPoint();
        }
        else
        {
            enemy.transform.position = transform.position;
        }
            enemy.GetComponent<EnemyController>().Init(
                cfg.enemyNormalHP,
                cfg.scoreNormalEnemy,
                cfg.enemySpeed,
                cfg.enemyInvicibleTime

            );
    }
}
