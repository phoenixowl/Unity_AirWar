using UnityEngine;

public class EnemySpawnArea : MonoBehaviour
{
    private BoxCollider2D box;

    void Awake()
    {
        box = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// 在区域内随机返回一个世界坐标
    /// </summary>
    public Vector3 GetRandomPoint()
    {
        Bounds bounds = box.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(x, y, 0f);
    }
}