using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    float timer;
    float spawnInterval = 10f;
    [SerializeField] private EnemySpawnArea spawnArea;

    StatsConfigSO cfg;

    private void Start()
    {
        cfg = ConfigManager.Instance.StatsConfigSO;
        spawnInterval = cfg.itemSpawnInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0;
            SpawnItem();

            spawnInterval = GetItemSpawnInterval(GameStateManager.Instance.gameState);
            Debug.Log("生成一个道具，下一次生成预计" + spawnInterval);
        }
    }

    float GetItemSpawnInterval(float gameTime)
    {
        float totalTime = cfg.expectedGameTime;
        float minInterval = cfg.itemSpawnInterval / 3.0f;
        float maxInterval = cfg.itemSpawnInterval;

        float t = Mathf.Clamp01(gameTime / totalTime);

        //f(0)=0, f(1)=1, f'(0)=0, f'(1)=0
        float smoothT = t * t * t * (t * (6f * t - 15f) + 10f);

        // 反转：开始时间隔大，结束时间隔小
        return Mathf.Lerp(minInterval, maxInterval, 1f - smoothT);
    }

    void SpawnItem()
    {
        GameObject item = ObjectPool.Instance.GetItem();


        if (spawnArea)
        {
            item.transform.position = spawnArea.GetRandomPoint();
        }
        else
        {
            item.transform.position = transform.position;
        }
        item.GetComponent<ItemController>().Init(
            cfg.itemMoveSpeed,
            UnityEngine.Random.Range(0, 3)
        );
    }
}
