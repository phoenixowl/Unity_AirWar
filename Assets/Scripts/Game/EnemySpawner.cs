using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    float timer;
    float spawnInterval = 1.5f;
   [SerializeField] private EnemySpawnArea enemySpawnArea;

    StatsConfigSO cfg;

    private void Start()
    {
        cfg = ConfigManager.Instance.StatsConfigSO;
        spawnInterval = cfg.enemySpawnInterval;
}

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0;
            SpawnEnemy();

            spawnInterval = GetEnemySpawnInterval(GameStateManager.Instance.gameState);
        }
    }

    float GetEnemySpawnInterval(float gameTime)
    {
        float totalTime = cfg.expectedGameTime;
        float minInterval = cfg.enemySpawnInterval / 3.0f;
        float maxInterval = cfg.enemySpawnInterval;

        float t = Mathf.Clamp01(gameTime / totalTime);

        //f(0)=0, f(1)=1, f'(0)=0, f'(1)=0
        float smoothT = t * t * t * (t * (6f * t - 15f) + 10f);

        // 反转：开始时间隔大，结束时间隔小
        return Mathf.Lerp(minInterval, maxInterval, 1f - smoothT);
    }

    void SpawnEnemy()
    {
        GameObject enemy = ObjectPool.Instance.GetEnemyNormal();

        float r = UnityEngine.Random.Range(0.0f, 1.0f);

        bool isElite = false;
        if (r < cfg.eliteEnemyRate)
        {
            isElite = true;
        }

        if (enemySpawnArea)
        {
            enemy.transform.position = enemySpawnArea.GetRandomPoint();
        }
        else
        {
            enemy.transform.position = transform.position;
        }
            enemy.GetComponent<EnemyController>().Init(
                Convert.ToInt32(cfg.scoreEnemy * GameStateManager.Instance.GameSpeed),
                cfg.enemyHP,
                cfg.enemyMoveSpeed,
                cfg.enemyFireInterval,
                cfg.enemyTouchDamage,
                cfg.enemyBulletDamage,
                cfg.enemyBulletSpeed,
                cfg.enemyInvicibleTime,
                isElite

            );
    }
}
