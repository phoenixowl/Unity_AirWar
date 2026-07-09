using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // ===== 单例 =====
    public static ObjectPool Instance { get; private set; }

    // ===== Inspector 配置 =====
    [Header("子弹池")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int bulletPoolSize = 30;

    [Header("普通敌机池")]
    [SerializeField] GameObject enemyNormalPrefab;
    [SerializeField] int enemyNormalPoolSize = 20;

    [Header("精英敌机池")]
    [SerializeField] GameObject enemyElitePrefab;
    [SerializeField] int enemyElitePoolSize = 10;

    // ===== 内部池 =====
    private Queue<GameObject> bulletPool = new();
    private Queue<GameObject> enemyNormalPool = new();
    private Queue<GameObject> enemyElitePool = new();

    void Awake()
    {
        // 单例初始化
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 预加载
        Prewarm(bulletPrefab, bulletPool, bulletPoolSize);
        //Prewarm(enemyNormalPrefab, enemyNormalPool, enemyNormalPoolSize);
        //Prewarm(enemyElitePrefab, enemyElitePool, enemyElitePoolSize);
    }

    /// <summary>
    /// 预加载对象
    /// </summary>
    private void Prewarm(GameObject prefab, Queue<GameObject> pool, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    // ================= 对外接口 =================

    public GameObject GetBullet()
    {
        return GetFromPool(bulletPrefab, bulletPool);
    }

    public void ReturnBullet(GameObject obj)
    {
        ReturnToPool(obj, bulletPool);
    }

    public GameObject GetEnemyNormal()
    {
        return GetFromPool(enemyNormalPrefab, enemyNormalPool);
    }

    public void ReturnEnemyNormal(GameObject obj)
    {
        ReturnToPool(obj, enemyNormalPool);
    }

    public GameObject GetEnemyElite()
    {
        return GetFromPool(enemyElitePrefab, enemyElitePool);
    }

    public void ReturnEnemyElite(GameObject obj)
    {
        ReturnToPool(obj, enemyElitePool);
    }

    // ================= 内部逻辑 =================

    private GameObject GetFromPool(GameObject prefab, Queue<GameObject> pool)
    {
        if (pool.Count == 0)
        {
            // 池空了，动态扩容（防止极端情况）
            GameObject obj = Instantiate(prefab, transform);
            return obj;
        }

        GameObject go = pool.Dequeue();
        go.SetActive(true);
        return go;
    }

    private void ReturnToPool(GameObject obj, Queue<GameObject> pool)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform); // 归位，防止脏层级
        pool.Enqueue(obj);
    }
}